using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace _3D_Game
{
    public class CollisionSolver
    {
        // SAT Collision test for OBBs
        public static bool ObbSAT(BoundVolume A, BoundVolume B, out Collision col)
        {
            col = new Collision();

            // parameters needed for collision response
            float   cDist = 0.0f;               // minimum collision penetration distance
            Vector3 cAxis = Vector3.Zero;       // corresponding collision axis

            // Create rotation matrix from B to A, and its absolute value version
            float[,] R = new float[3, 3];
            float[,] AbsR = new float[3, 3];
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    R[i, j] = Vector3.Dot(A.axes[i], B.axes[j]);
                    AbsR[i, j] = Math.Abs(R[i, j]) + Globals.mEpsilon;
                }

            // Create translation vector to A's coordinate frame
            Vector3 t = B.center - A.center;
            t = new Vector3(Vector3.Dot(t, A.axes[0]),
                            Vector3.Dot(t, A.axes[1]),
                            Vector3.Dot(t, A.axes[2]));
            
            // Now for the tests, hardcoded (and ugly) for speedier performance
            #region tests
            float rA, rB, TL;                   // projected radii for A and B, and translate distance

            // 1
            rA = A.ex.X;
            rB = B.ex.X * AbsR[0, 0] + B.ex.Y * AbsR[0, 1] + B.ex.Z * AbsR[0, 2];
            TL = Math.Abs(t.X) - (rA + rB);
            if (TL > 0) return false;
            else if (Math.Abs(TL) < cDist)
            {
                cDist = Math.Abs(TL);
                cAxis = A.axes[0];
            }

            // 2
            rA = A.ex.Y;
            rB = B.ex.X * AbsR[1, 0] + B.ex.Y * AbsR[1, 1] + B.ex.Z * AbsR[1, 2];
            TL = Math.Abs(t.Y) - (rA + rB);
            if (TL > 0) return false;
            else if (Math.Abs(TL) < cDist)
            {
                cDist = Math.Abs(TL);
                cAxis = A.axes[1];
            }

            // 3
            rA = A.ex.Z;
            rB = B.ex.X * AbsR[2, 0] + B.ex.Y * AbsR[2, 1] + B.ex.Z * AbsR[2, 2];
            TL = Math.Abs(t.Z) - (rA + rB);
            if (TL > 0) return false;
            else if (Math.Abs(TL) < cDist)
            {
                cDist = Math.Abs(TL);
                cAxis = A.axes[2];
            }

            // 4
            rA = A.ex.X * AbsR[0, 0] + A.ex.Y * AbsR[1, 0] + A.ex.Z * AbsR[2, 0];
            rB = B.ex.X;
            TL = Math.Abs(t.X * R[0, 0] + t.Y * R[1, 0] + t.Z * R[2, 0]) - (rA + rB);
            if (TL > 0) return false;
            else if (Math.Abs(TL) < cDist)
            {
                cDist = Math.Abs(TL);
                cAxis = B.axes[0];
            }

            // 5
            rA = A.ex.X * AbsR[0, 1] + A.ex.Y * AbsR[1, 1] + A.ex.Z * AbsR[2, 1];
            rB = B.ex.Y;
            TL = Math.Abs(t.X * R[0, 1] + t.Y * R[1, 1] + t.Z * R[2, 1]) - (rA + rB);
            if (TL > 0) return false;
            else if (Math.Abs(TL) < cDist)
            {
                cDist = Math.Abs(TL);
                cAxis = B.axes[1];
            }

            // 6
            rA = A.ex.X * AbsR[0, 2] + A.ex.Y * AbsR[1, 2] + A.ex.Z * AbsR[2, 2];
            rB = B.ex.Z;
            TL = Math.Abs(t.X * R[0, 2] + t.Y * R[1, 2] + t.Z * R[2, 2]) - (rA + rB);
            if (TL > 0) return false;
            else if (Math.Abs(TL) < cDist)
            {
                cDist = Math.Abs(TL);
                cAxis = B.axes[2];
            }

            // 7
            rA = A.ex.Y * AbsR[2, 0] + A.ex.Z * AbsR[1, 0];
            rB = B.ex.Y * AbsR[0, 2] + B.ex.Z * AbsR[0, 1];
            TL = Math.Abs(t.Z * R[1, 0] - t.Y * R[2, 0]) - (rA + rB);
            if (TL > 0) return false;
            else if (Math.Abs(TL) < cDist)
            {
                cDist = Math.Abs(TL);
                cAxis = Vector3.Cross(A.axes[0], B.axes[0]);
            }

            // 8
            rA = A.ex.Y * AbsR[2, 1] + A.ex.Z * AbsR[1, 1];
            rB = B.ex.X * AbsR[0, 2] + B.ex.Z * AbsR[0, 0];
            TL = Math.Abs(t.Z * R[1, 1] - t.Y * R[2, 1]) - (rA + rB);
            if (TL > 0) return false;
            else if (Math.Abs(TL) < cDist)
            {
                cDist = Math.Abs(TL);
                cAxis = Vector3.Cross(A.axes[0], B.axes[1]);
            }

            // 9
            rA = A.ex.Y * AbsR[2, 2] + A.ex.Z * AbsR[1, 2];
            rB = B.ex.X * AbsR[0, 1] + B.ex.Y * AbsR[0, 0];
            TL = Math.Abs(t.Z * R[1, 2] - t.Y * R[2, 2]) - (rA + rB);
            if (TL > 0) return false;
            else if (Math.Abs(TL) < cDist)
            {
                cDist = Math.Abs(TL);
                cAxis = Vector3.Cross(A.axes[0], B.axes[2]);
            }

            // 10
            rA = A.ex.X * AbsR[2, 0] + A.ex.Z * AbsR[0, 0];
            rB = B.ex.Y * AbsR[1, 2] + B.ex.Z * AbsR[1, 1];
            TL = Math.Abs(t.X * R[2, 0] - t.Z * R[0, 0]) - (rA + rB);
            if (TL > 0) return false;
            else if (Math.Abs(TL) < cDist)
            {
                cDist = Math.Abs(TL);
                cAxis = Vector3.Cross(A.axes[1], B.axes[0]);
            }

            // 11
            rA = A.ex.X * AbsR[2, 1] + A.ex.Z * AbsR[0, 1];
            rB = B.ex.X * AbsR[1, 2] + B.ex.Z * AbsR[1, 0];
            TL = Math.Abs(t.X * R[2, 1] - t.Z * R[0, 1]) - (rA + rB);
            if (TL > 0) return false;
            else if (Math.Abs(TL) < cDist)
            {
                cDist = Math.Abs(TL);
                cAxis = Vector3.Cross(A.axes[1], B.axes[1]);
            }

            // 12
            rA = A.ex.X * AbsR[2, 2] + A.ex.Z * AbsR[0, 2];
            rB = B.ex.X * AbsR[1, 2] + B.ex.Z * AbsR[1, 0];
            TL = Math.Abs(t.X * R[2, 2] - t.Z * R[0, 2]) - (rA + rB);
            if (TL > 0) return false;
            else if (Math.Abs(TL) < cDist)
            {
                cDist = Math.Abs(TL);
                cAxis = Vector3.Cross(A.axes[1], B.axes[2]);
            }

            // 13
            rA = A.ex.X * AbsR[1, 0] + A.ex.Y * AbsR[0, 0];
            rB = B.ex.Y * AbsR[2, 2] + B.ex.Z * AbsR[2, 1];
            TL = Math.Abs(t.Y * R[0, 0] - t.X * R[1, 0]) - (rA + rB);
            if (TL > 0) return false;
            else if (Math.Abs(TL) < cDist)
            {
                cDist = Math.Abs(TL);
                cAxis = Vector3.Cross(A.axes[2], B.axes[0]);
            }

            // 14
            rA = A.ex.X * AbsR[1, 1] + A.ex.Y * AbsR[0, 1];
            rB = B.ex.X * AbsR[2, 2] + B.ex.Z * AbsR[2, 0];
            TL = Math.Abs(t.Y * R[0, 1] - t.X * R[1, 1]) - (rA + rB);
            if (TL > 0) return false;
            else if (Math.Abs(TL) < cDist)
            {
                cDist = Math.Abs(TL);
                cAxis = Vector3.Cross(A.axes[2], B.axes[1]);
            }

            // 15
            rA = A.ex.X * AbsR[1, 2] + A.ex.Y * AbsR[0, 2];
            rB = B.ex.X * AbsR[2, 1] + B.ex.Y * AbsR[2, 0];
            TL = Math.Abs(t.Y * R[0, 2] - t.X * R[1, 2]) - (rA + rB);
            if (TL > 0) return false;
            else if (Math.Abs(TL) < cDist)
            {
                cDist = Math.Abs(TL);
                cAxis = Vector3.Cross(A.axes[2], B.axes[2]);
            }
            #endregion

            // If we got here, there was a collision. Find the contact points
            Vector3[] hitPoints;
            int numHitPoints;
            FindContact(A, B, cDist, cAxis, out hitPoints, out numHitPoints);

            col = new Collision(cDist, cAxis, hitPoints, numHitPoints);

            //// Test all 15 axes
            //for (int i = 0; i < 3; i++)
            //{
            //    if (!AxisTest(A, B, A.axes[i], out cDist, out cAxis)) return false;
            //    if (!AxisTest(A, B, B.axes[i], out cDist, out cAxis)) return false;

            //    for (int j = 0; j < 3; j++)
            //    {
            //        Vector3 test = Vector3.Cross(A.axes[i], B.axes[j]);
            //        if (!AxisTest(A, B, test, out cDist, out cAxis)) return false;
            //    }
            //}

            return true;
        }

        // CalculateHitPoint
        // p: distance of penetration
        // norm: axis of penetration
        // hitPoints: vertices of contact manifold
        // numHitPoints: number of contact vertices
        public static void FindContact(BoundVolume A, BoundVolume B, float p, Vector3 norm, out Vector3[] hitPoints, out int numHitPoints)
        {
            Vector3[] vA, vB;
           
            int[] vIndexA, vIndexB;
            int vCountA = FindContactVertices(A, p, norm, out vA, out vIndexA);
            int vCountB = FindContactVertices(B, p, norm, out vB, out vIndexB);

            int vCountX = vCountA;
            Vector3[] vX = vA;

            if (vCountA >= 4 && vCountB >= 4)
            {
                Vector3[] clipVerts = new Vector3[50];  // ouch
                ClipFaceFaceVerts(vA, vIndexA, vB, vIndexB, out clipVerts, out vCountX);
                vX = clipVerts; // i don't understand why this needs to happen
            }

            // branchinggggggg
            {
                if (vCountB < vCountA)
                {
                    vCountX = vCountB;
                    vX = vB;
                    //norm = norm;
                }

                if (vCountB == 2 && vCountA == 2)
                {
                    Vector3[] V = new Vector3[2];
                    int numV = 0;

                    ClosestPointLineLine(vA, vB, ref V, ref numV);
                    vX = V;
                    vCountX = numV;
                }

                if (vCountA == 2 && vCountB == 4)
                {
                    ClosestPointPointOBB(vA[0], B, ref vX[0]);
                    ClosestPointPointOBB(vA[1], B, ref vX[1]);
                    vCountX = 2;
                }

                if (vCountA == 4 && vCountB == 2)
                {
                    ClosestPointPointOBB(vB[0], A, ref vX[0]);
                    ClosestPointPointOBB(vB[1], A, ref vX[1]);
                    vCountX = 2;
                }

                numHitPoints = vCountX;
                hitPoints = new Vector3[vCountX];
                for (int i = 0; i < vCountX; i++)
                {
                    hitPoints[i] = vX[i];
                }
            }
        }

        // Find the vertices of the given box that are inside the collision volume
        public static int FindContactVertices(BoundVolume A, float p, Vector3 norm, out Vector3[] vs, out int[] vsi)
        {
            // construct box vertices from the OBB data
            Vector3[] verts = new Vector3[8];
            verts[0] = -A.ex.X * A.axes[0] - A.ex.Y * A.axes[1] - A.ex.Z * A.axes[2];
            verts[1] =  A.ex.X * A.axes[0] - A.ex.Y * A.axes[1] - A.ex.Z * A.axes[2];
            verts[2] =  A.ex.X * A.axes[0] - A.ex.Y * A.axes[1] + A.ex.Z * A.axes[2];
            verts[3] = -A.ex.X * A.axes[0] - A.ex.Y * A.axes[1] + A.ex.Z * A.axes[2];
            verts[4] = -A.ex.X * A.axes[0] + A.ex.Y * A.axes[1] - A.ex.Z * A.axes[2];
            verts[5] =  A.ex.X * A.axes[0] + A.ex.Y * A.axes[1] - A.ex.Z * A.axes[2];
            verts[6] =  A.ex.X * A.axes[0] + A.ex.Y * A.axes[1] + A.ex.Z * A.axes[2];
            verts[7] = -A.ex.X * A.axes[0] + A.ex.Y * A.axes[1] + A.ex.Z * A.axes[2];

            Vector3 pp = verts[0];
            float maxDist = Vector3.Dot(pp, norm);
            for (int i = 1; i < 8; i++)
            {
                float d = Vector3.Dot(verts[i], norm);
                if (d > maxDist)
                {
                    maxDist = d;
                    pp = verts[i];
                }
            }

            maxDist -= (p + Globals.cvEpsilon);

            vs = new Vector3[8];
            vsi = new int[8];

            int numVerts = 0;
            for (int i = 0; i < 8; i++)
            {
                float side = Vector3.Dot(verts[i], norm) - maxDist;
                if (side > 0)
                {
                    vs[numVerts] = verts[i];
                    vsi[numVerts] = i;
                    numVerts++;
                }
            }

            return numVerts;
        }

        // Sort vertices in clockwise order
        public static void SortVertices(ref Vector3[] verts, ref int[] vIndex)
        {
            int[,] faces = new int[6, 4];
            faces[0, 0] = 4; faces[0, 1] = 0; faces[0, 2] = 3; faces[0, 3] = 7;
            faces[1, 0] = 1; faces[1, 1] = 5; faces[1, 2] = 6; faces[1, 3] = 2;
            faces[2, 0] = 0; faces[2, 1] = 1; faces[2, 2] = 2; faces[2, 3] = 3;
            faces[3, 0] = 7; faces[3, 1] = 6; faces[3, 2] = 5; faces[3, 3] = 4;
            faces[4, 0] = 5; faces[4, 1] = 1; faces[4, 2] = 0; faces[4, 3] = 4;
            faces[5, 0] = 6; faces[5, 1] = 7; faces[5, 2] = 3; faces[5, 3] = 2;

            int faceSet = -1;
            int numFound = 0;
            Vector3[] temp = new Vector3[4];

            for (int i = 0; i < 6 && faceSet == -1; i++)
            {
                numFound = 0;
                for (int j = 0; j < 4; j++)
                {
                    if (vIndex[j] == faces[i,j])
                    {
                        temp[numFound] = verts[j];
                        numFound++;

                        if (numFound == 4)
                        {
                            faceSet = i;
                            break;
                        }
                    }
                }
            }

            if (faceSet < 0)
            {
                // something bad happened?
            }

            else
            {
                for (int i = 0; i < 4; i++)
                {
                    verts[i] = temp[i];
                }
            }
        }

        // Is vertex inside a face?
        public static bool VertInsideFace(Vector3[] verts0, Vector3 p0, float planeErr = 0.0f)
        {
            Vector3 n = Vector3.Cross(verts0[2] - verts0[0], verts0[1] - verts0[0]);
            n.Normalize();

            for (int i = 0; i < 4; i++)
            {
                Vector3 s0 = verts0[i];
                Vector3 s1 = verts0[(i + 1) % 4];
                Vector3 sx = s0 + n * 10.0f;

                Vector3 sn = Vector3.Cross(sx - s0, s1 - s0);
                sn.Normalize();

                float d0 = Vector3.Dot(p0, sn) - Vector3.Dot(s0, sn);
                if (d0 > planeErr)
                    return false;
            }

            return true;
        }

        // Clip 2 faces
        public static void ClipFaceFaceVerts(Vector3[] verts0, int[] vIndex0, Vector3[] verts1, int[] vIndex1, out Vector3[] vertsX, out int numX)
        {
            SortVertices(ref verts0, ref vIndex0);
            SortVertices(ref verts1, ref vIndex1);

            Vector3 n = Vector3.Cross(verts0[2] - verts0[0], verts0[1] - verts0[0]);
            n.Normalize();

            // Project all vertices onto shared plane
            Vector3[] vertsTemp1 = new Vector3[4];
            for (int i = 0; i < 4; i++)
            {
                vertsTemp1[i] = verts1[i] + (n * Vector3.Dot(n, verts0[0] - verts1[i]));
            }

            // Wasteful
            Vector3[] temp = new Vector3[50];
            int numVerts = 0;

            // Big loop
            for (int c = 0; c < 2; c++)
            {
                Vector3[] vertA = (c == 1) ? verts0 : vertsTemp1;
                Vector3[] vertB = (c == 1) ? vertsTemp1 : verts0;

                // face normal
                n = Vector3.Cross(vertA[2] - vertA[0], vertA[1] - vertA[0]);
                n.Normalize();

                for (int i = 0; i < 4; i++)
                {
                    Vector3 s0 = vertA[i];
                    Vector3 s1 = vertA[(i + 1) % 4];
                    Vector3 sx = s0 + n * 10.0f;

                    // face normal again
                    Vector3 sn = Vector3.Cross(sx - s0, s1 - s0);
                    sn.Normalize();

                    float d = Vector3.Dot(s0, sn);

                    for (int j = 0; j < 4; j++)
                    {
                        Vector3 p0 = vertB[j];
                        Vector3 p1 = vertB[(j + 1) % 4];
                        float d0 = Vector3.Dot(p0, sn) - d;
                        float d1 = Vector3.Dot(p1, sn) - d;

                        // Check if they're on opposite sides of plane
                        if ((d0 * d1) < 0.0f)
                        {
                            Vector3 pX = p0 + (Vector3.Dot(sn, (s0 - p0)) / Vector3.Dot(sn, (p1 - p0))) * (p1 - p0);

                            if (VertInsideFace(vertA, pX, 0.1f))
                            {
                                temp[numVerts] = pX;
                                numVerts++;
                            }
                        }

                        if (VertInsideFace(vertA, p0))
                            if (true)
                            {
                                temp[numVerts] = p0;
                                numVerts++;
                            }

                        // so very very wasteful
                        if (numVerts > 40)
                        {
                            // do something ..
                        }
                    }
                }
            }

            // Remove vertices that are too close to each other
            for (int i = 0; i < numVerts; i++)
                for (int j = 1; j < numVerts; j++)
                    if (i != j)
                    {
                        Vector3 L = temp[i] - temp[j];
                        float dist = L.LengthSquared();

                        if (dist < Globals.vEpsilon)
                        {
                            for (int k = j; k < numVerts; k++)
                                temp[k] = temp[k + 1];
                            numVerts--;
                        }
                    }

            // Finally, pass back all vertices and count
            numX = numVerts;
            vertsX = new Vector3[numVerts];
            for (int i = 0; i < numVerts; i++)
            {
                vertsX[i] = temp[i];
            }
        }
    
        // Closest point Edge-Edge
        public static void ClosestPointLineLine(Vector3[] verts0, Vector3[] verts1, ref Vector3[] vertsX, ref int numVertX)
        {
            Vector3 p1 = verts0[0];
            Vector3 q1 = verts0[1];
            Vector3 p2 = verts1[0];
            Vector3 q2 = verts1[1];

            Vector3 d1 = q1 - p1;
            Vector3 d2 = q2 - p2;
            Vector3 r = p1 = p2;

            float a = Vector3.Dot(d1, d2);
            float e = Vector3.Dot(d2, d2);
            float f = Vector3.Dot(d2, r);

            float s, t;

            if (a <= Globals.eeEpsilon && e <= Globals.eeEpsilon)
            {
                // why do we even need these ... nothing happens after we return
                //s = 0.0f;
                //t = 0.0f;
                //c1 = p1;
                //c2 = p2;

                vertsX[0] = p1;
                numVertX = 1;
                return;
            }
            if (a <= Globals.eeEpsilon)
            {
                s = 0.0f;
                t = Clamp(f / e, 0.0f, 1.0f);
            }
            else
            {
                float c = Vector3.Dot(d1, r);
                if (e <= Globals.eeEpsilon)
                {
                    t = 0.0f;
                    s = Clamp(-c / a, 0.0f, 1.0f);
                }
                else
                {
                    float b = Vector3.Dot(d1, d2);  // in other words ... = a?
                    float denom = a * e - b * b;

                    if (denom != 0.0f)
                        s = Clamp((b * f - c * e) / denom, 0.0f, 1.0f);
                    else
                        s = 0.0f;

                    t = (b * s + f) / e;

                    if (t < 0.0f)
                    {
                        t = 0.0f;
                        s = Clamp(-c / a, 0.0f, 1.0f);
                    }
                    else if (t > 1.0f)
                    {
                        t = 1.0f;
                        s = Clamp((b - c) / a, 0.0f, 1.0f);
                    }
                }
            }
        }

        // Closest point Point-OBB
        public static void ClosestPointPointOBB(Vector3 point, BoundVolume box0, ref Vector3 closestP)
        {
            Vector3 d = point - box0.center;
            Vector3 q = box0.center;

            for (int i = 0; i < 3; i++)
            {
                float dist = Vector3.Dot(d, box0.axes[i]);
                if (dist > box0.X(i)) dist = box0.X(i);
                if (dist < -box0.X(i)) dist = -box0.X(i);
                q += dist * box0.axes[i];
            }

            closestP = q;
        }

        // Clamp within max/min range
        private static float Clamp(float v, float min, float max)
        {
            float res = v;
            res = (v > max) ? max : v;
            res = (v < min) ? min : v;
            return res;
        }
    
    }
}
