using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UR3Solver
{
    //change your DH parameters to match the scaling in unity
    private float DH_ratio = 4.185057f / 0.18445f; //taken from length of DH_a
    //DH_d[4] was changed to -.119 from .109 to accomidate axis change (and .119 fit better for unity), and DH_d[5] was changed slightly as well    
    private float[] DH_d = { 0f, .0475f, 0f, 0f, -.04784f, .05375f, .02228f }; //Change values DH_d y DH_a to construct a new system 
    private float[] DH_a = { 0f, 0f, -.18445f, -.17217f, 0f, 0f, 0f }; //Use meters for the measures
    private float[] DH_alph = { 0f, (float)(Math.PI / 2), 0f, 0f, (float)(Math.PI / 2), (float)(-Math.PI / 2), 0f };

    //contains final solutions for Unity
    private float[,] solutionMatrix = new float[6, 8];
    public float[] solutionArray = new float[6];

    private float x, y, z, phi, theta, psi;

    public UR3Solver()
    {
        for (int i = 0; i < 6; i++)
        {
            solutionArray[i] = 0;

            for (int j = 0; j < 8; j++)
            {
                solutionMatrix[i, j] = 0;
            }
        }

        x = y = z = phi = theta = psi = 0;

        DHConvert();
    }

    private void DHConvert() //unity units for this model is different from real world size
    {
        for (int i = 0; i < 7; i++)
        {
            DH_d[i] *= DH_ratio;
            DH_a[i] *= DH_ratio;
        }
    }

    public void Solve(float x2, float y2, float z2, float phi2, float theta2, float psi2)
    {
        Matrix4x4 efPos;
        this.x = x2;
        this.psi = phi2;
        this.y = -z2;
        this.theta = -psi2;
        this.z = y2;
        this.phi = theta2;

        efPos = hMatrix();

        IK_Solver(efPos);

        for (int i = 0; i < 6; i++)
        {
            this.solutionArray[i] = this.solutionMatrix[i, 0] * Mathf.Rad2Deg;
        }
    }

    //create your homogeneous transformation matrix
    //may need to update based on other transformation matrix
    private Matrix4x4 hMatrix()
    {
        //roll along z, pitch along y, yaw along x
        //phi = roll, theta = pitch, psi = yaw
        Matrix4x4 translate = Matrix4x4.identity;
        Matrix4x4 yaw = Matrix4x4.identity;
        Matrix4x4 pitch = Matrix4x4.identity;
        Matrix4x4 roll = Matrix4x4.identity;
        Matrix4x4 matrix;

        //set the translation matrix
        translate.m03 = x;
        translate.m13 = y;
        translate.m23 = z;

        //set roll matrix (z rotation)
        roll.m00 = (float)Math.Cos(phi);
        roll.m01 = -(float)Math.Sin(phi);
        roll.m10 = (float)Math.Sin(phi);
        roll.m11 = (float)Math.Cos(phi);

        //set pitch matrix (y rotation)
        pitch.m00 = (float)Math.Cos(theta);
        pitch.m02 = (float)Math.Sin(theta);
        pitch.m20 = -(float)Math.Sin(theta);
        pitch.m22 = (float)Math.Cos(theta);

        //set yaw matrix (x rotation)
        yaw.m11 = (float)Math.Cos(psi);
        yaw.m12 = -(float)Math.Sin(psi);
        yaw.m21 = (float)Math.Sin(psi);
        yaw.m22 = (float)Math.Cos(psi);

        //will need to set end effector rotation matrix
        /*        matrix = roll * pitch * yaw * translate;    */
        matrix = translate * roll * pitch * yaw;

        return matrix;
    }

    //each transformation from link i-1 to link i will use the following transformation
    //note: DH params[0] is the robot base, but the solution matrix[0] is joint 1 theta
    private Matrix4x4 aMatrix(int row, int column)
    {
        Matrix4x4 matrix = Matrix4x4.identity;

        matrix.m00 = (float)Math.Cos(this.solutionMatrix[row - 1, column]);
        matrix.m01 = -(float)Math.Sin(this.solutionMatrix[row - 1, column]);
        matrix.m03 = DH_alph[row - 1];

        matrix.m10 = (float)Math.Sin(this.solutionMatrix[row - 1, column]) * (float)Math.Cos(DH_alph[row - 1]);
        matrix.m11 = (float)Math.Cos(this.solutionMatrix[row - 1, column]) * (float)Math.Cos(DH_alph[row - 1]);
        matrix.m12 = -(float)Math.Sin(DH_alph[row - 1]);
        matrix.m13 = -(float)Math.Sin(DH_alph[row - 1]) * DH_d[row];

        matrix.m20 = (float)Math.Sin(this.solutionMatrix[row - 1, column]) * (float)Math.Sin(DH_alph[row - 1]);
        matrix.m21 = (float)Math.Cos(this.solutionMatrix[row - 1, column]) * (float)Math.Sin(DH_alph[row - 1]);
        matrix.m22 = (float)Math.Cos(DH_alph[row - 1]);
        matrix.m23 = (float)Math.Cos(DH_alph[row - 1]) * DH_d[row];

        return matrix;
    }

    private void IK_Solver(Matrix4x4 efPos)
    {
        float theta1, theta2, theta3, theta4, theta5, theta6;
        float sin1, cos1, sin5, p14Norm;
        Vector4 p05, d6, p14, p06;
        Vector2 yHat, xHat;
        Matrix4x4 efInverse, T01, T16, T65, T54, T14, T21, T32, T34;

        //******** Theta1

        d6.x = 0;
        d6.y = 0;
        d6.z = DH_d[6];
        d6.w = 1;

        p05 = efPos * d6;
        p06.x = efPos.m03;
        p06.y = efPos.m13;
        p06.z = efPos.m23;
        p06.w = efPos.m33;

        //first theta1 calculation with the positive square root
        theta1 = (float)(Math.Atan2(p05.y, p05.x) + Math.Acos(DH_d[4] / Math.Sqrt(Math.Pow(p05.x, 2) + Math.Pow(p05.y, 2))) + Math.PI / 2);
        //Debug.Log("theta1 = " + theta1 * Mathf.Rad2Deg);

        //fill in the first four elements of the first row of the solution matrix with theta1
        for (int i = 0; i < 4; i++)
        {
            this.solutionMatrix[0, i] = theta1;
        }

        //recalculate theta1 for the negative square root and store
        theta1 = (float)(Math.Atan2(p05.y, p05.x) - Math.Acos(DH_d[4] / Math.Sqrt(Math.Pow(p05.x, 2) + Math.Pow(p05.y, 2))) + Math.PI / 2);
        for (int i = 4; i < 8; i++)
        {
            this.solutionMatrix[0, i] = theta1;
        }

        //******** Theta5

        //need to take care of case when theta1 is positive sqrt
        sin1 = (float)Math.Sin(this.solutionMatrix[0, 0]);
        cos1 = (float)Math.Cos(this.solutionMatrix[0, 0]);

        //calculate positive solution
        theta5 = (float)Math.Acos(-1 * (p06.x * sin1 - p06.y * cos1 - DH_d[4]) / DH_d[6]);
        this.solutionMatrix[4, 0] = this.solutionMatrix[4, 1] = theta5;

        //now store negative solution
        this.solutionMatrix[4, 2] = this.solutionMatrix[4, 3] = -theta5;

        //need to take care of case when theta1 is negative sqrt
        sin1 = (float)Math.Sin(this.solutionMatrix[0, 4]);
        cos1 = (float)Math.Cos(this.solutionMatrix[0, 4]);
        theta5 = (float)Math.Acos(-1 * (p06.x * sin1 - p06.y * cos1 - DH_d[4]) / DH_d[6]);

        this.solutionMatrix[4, 4] = this.solutionMatrix[4, 5] = theta5;
        this.solutionMatrix[4, 6] = this.solutionMatrix[4, 7] = -theta5;

        //******** Theta6

        //if sin(theta5) == 0, then can set to arbitrary value
        if (Math.Sin(theta5) == 0)
        {
            for (int i = 0; i < 8; i++)
            {
                this.solutionMatrix[5, i] = (float)Math.PI / 2;
            }
        }

        efInverse = efPos.inverse;
        xHat.x = efInverse.m00;
        xHat.y = efInverse.m10;
        yHat.x = efInverse.m01;
        yHat.y = efInverse.m11;

        //theta1 is positive sqrt and theta5 is positive
        sin5 = (float)Math.Sin(this.solutionMatrix[4, 0]);
        sin1 = (float)Math.Sin(this.solutionMatrix[0, 0]);
        cos1 = (float)Math.Cos(this.solutionMatrix[0, 0]);

        theta6 = (float)Math.Atan2(((-xHat[1] * sin1 + yHat[1] * cos1) / sin5), ((xHat[0] * sin1 - yHat[0] * cos1) / sin5));
        this.solutionMatrix[5, 0] = this.solutionMatrix[5, 1] = theta6;

        //theta5 is negative
        sin5 = (float)Math.Sin(this.solutionMatrix[4, 2]);
        theta6 = (float)Math.Atan2(((-xHat[1] * sin1 + yHat[1] * cos1) / sin5), ((xHat[0] * sin1 - yHat[0] * cos1) / sin5));
        this.solutionMatrix[5, 2] = this.solutionMatrix[5, 3] = theta6;

        //theta1 is negative sqrt and theta5 is positive
        sin5 = (float)Math.Sin(this.solutionMatrix[4, 4]);
        sin1 = (float)Math.Sin(this.solutionMatrix[0, 4]);
        cos1 = (float)Math.Cos(this.solutionMatrix[0, 4]);

        theta6 = (float)Math.Atan2(((-xHat[1] * sin1 + yHat[1] * cos1) / sin5), ((xHat[0] * sin1 - yHat[0] * cos1) / sin5));
        this.solutionMatrix[5, 4] = this.solutionMatrix[5, 5] = theta6;

        //theta1 is negative sqrt and theta5 is negative
        sin5 = (float)Math.Sin(this.solutionMatrix[4, 6]);

        theta6 = (float)Math.Atan2(((-xHat[1] * sin1 + yHat[1] * cos1) / sin5), ((xHat[0] * sin1 - yHat[0] * cos1) / sin5));
        this.solutionMatrix[5, 6] = this.solutionMatrix[5, 7] = theta6;

        //******** Theta3

        //T06 = T01 * T16, so T16 = T10 * T06
        //because thetas 1, 5, and 6 are different in different columns, will need to solve across all columns
        for (int i = 0; i < 8; i++)
        {
            T01 = aMatrix(1, i); //transformation from T0 to T1 - need to pass in 1 since we are using the first joint as reference
            T16 = T01.inverse * efPos;

            //T65 = T56.i = aMatrix(theta6).inverse
            T65 = aMatrix(6, i).inverse;
            T54 = aMatrix(5, i).inverse;

            T14 = T16 * T65 * T54;

            //need to get the magnitude of the translation
            p14.x = T14.m03;
            p14.y = T14.m13; //make sure this works as a negative
            p14.z = T14.m23;
            p14.w = 1;

            p14Norm = (float)Math.Sqrt(Math.Pow(p14.x, 2) + Math.Pow(p14.z, 2));

            //will be negative for odd indices
            if (i % 2 == 0)
            {
                theta3 = (float)Math.Acos((Math.Pow(p14Norm, 2) - Math.Pow(DH_a[2], 2) - Math.Pow(DH_a[3], 2)) / (2 * DH_a[2] * DH_a[3]));
                theta2 = (float)Math.Atan2(-p14.z, -p14.x) - (float)Math.Asin(-DH_a[3] * (float)Math.Sin(theta3) / p14Norm);
                T32 = aMatrix(3, i).inverse;
                T21 = aMatrix(2, i).inverse;
                T34 = T32 * T21 * T14;

                theta4 = (float)Math.Atan2(T34.m10, T34.m00);

                this.solutionMatrix[2, i] = theta3;
                this.solutionMatrix[1, i] = theta2;
                this.solutionMatrix[3, i] = theta4;
            }
            else
            {
                theta3 = -(float)Math.Acos((Math.Pow(p14Norm, 2) - Math.Pow(DH_a[2], 2) - Math.Pow(DH_a[3], 2)) / (2 * DH_a[2] * DH_a[3]));
                theta2 = (float)Math.Atan2(-p14.z, -p14.x) - (float)Math.Asin(-DH_a[3] * (float)Math.Sin(theta3) / p14Norm);
                T32 = aMatrix(3, i).inverse;
                T21 = aMatrix(2, i).inverse;
                T34 = T32 * T21 * T14;

                theta4 = (float)Math.Atan2(T34.m10, T34.m00);

                this.solutionMatrix[2, i] = theta3;
                this.solutionMatrix[1, i] = theta2;
                this.solutionMatrix[3, i] = theta4;
            }
        }  
    }
}
