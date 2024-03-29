namespace RayTracer.Engine.Turbulences;

//http://code.google.com/p/simplexnoise/source/browse/trunk/SimplexNoise/Noise.cs

// SimplexNoise for C#
// Author: Heikki Törmälä

//This is free and unencumbered software released into the public domain.

//Anyone is free to copy, modify, publish, use, compile, sell, or
//distribute this software, either in source code form or as a compiled
//binary, for any purpose, commercial or non-commercial, and by any
//means.

//In jurisdictions that recognize copyright laws, the author or authors
//of this software dedicate any and all copyright interest in the
//software to the public domain. We make this dedication for the benefit
//of the public at large and to the detriment of our heirs and
//successors. We intend this dedication to be an overt act of
//relinquishment in perpetuity of all present and future rights to this
//software under copyright law.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
//EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
//OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
//OTHER DEALINGS IN THE SOFTWARE.

//For more information, please refer to <http://unlicense.org/>
public class SimplexNoise
{
    /// <summary>
    /// 1D simplex noise
    /// </summary>
    /// <param name="x"></param>
    /// <returns></returns>
    public static float Generate(float x)
    {
        int i0 = FastFloor(x);
        int i1 = i0 + 1;
        float x0 = x - i0;
        float x1 = x0 - 1.0f;

        float n0, n1;

        float t0 = 1.0f - x0 * x0;
        t0 *= t0;
        n0 = t0 * t0 * grad(perm[i0 & 0xff], x0);

        float t1 = 1.0f - x1 * x1;
        t1 *= t1;
        n1 = t1 * t1 * grad(perm[i1 & 0xff], x1);
        // The maximum value of this noise is 8*(3/4)^4 = 2.53125
        // A factor of 0.395 scales to fit exactly within [-1,1]
        return 0.395f * (n0 + n1);
    }

    /// <summary>
    /// 2D simplex noise
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public static float Generate(float x, float y)
    {
        const float F2 = 0.366025403f; // F2 = 0.5*(sqrt(3.0)-1.0)
        const float G2 = 0.211324865f; // G2 = (3.0-Math.sqrt(3.0))/6.0

        float n0, n1, n2; // Noise contributions from the three corners

        // Skew the input space to determine which simplex cell we're in
        float s = (x + y) * F2; // Hairy factor for 2D
        float xs = x + s;
        float ys = y + s;
        int i = FastFloor(xs);
        int j = FastFloor(ys);

        float t = (float)(i + j) * G2;
        float X0 = i - t; // Unskew the cell origin back to (x,y) space
        float Y0 = j - t;
        float x0 = x - X0; // The x,y distances from the cell origin
        float y0 = y - Y0;

        // For the 2D case, the simplex shape is an equilateral triangle.
        // Determine which simplex we are in.
        int i1, j1; // Offsets for second (middle) corner of simplex in (i,j) coords
        if (x0 > y0) { i1 = 1; j1 = 0; } // lower triangle, XY order: (0,0)->(1,0)->(1,1)
        else { i1 = 0; j1 = 1; }      // upper triangle, YX order: (0,0)->(0,1)->(1,1)

        // A step of (1,0) in (i,j) means a step of (1-c,-c) in (x,y), and
        // a step of (0,1) in (i,j) means a step of (-c,1-c) in (x,y), where
        // c = (3-sqrt(3))/6

        float x1 = x0 - i1 + G2; // Offsets for middle corner in (x,y) unskewed coords
        float y1 = y0 - j1 + G2;
        float x2 = x0 - 1.0f + 2.0f * G2; // Offsets for last corner in (x,y) unskewed coords
        float y2 = y0 - 1.0f + 2.0f * G2;

        // Wrap the integer indices at 256, to avoid indexing perm[] out of bounds
        int ii = i % 256;
        int jj = j % 256;

        // Calculate the contribution from the three corners
        float t0 = 0.5f - x0 * x0 - y0 * y0;
        if (t0 < 0.0f) n0 = 0.0f;
        else
        {
            t0 *= t0;
            n0 = t0 * t0 * grad(perm[ii + perm[jj]], x0, y0);
        }

        float t1 = 0.5f - x1 * x1 - y1 * y1;
        if (t1 < 0.0f) n1 = 0.0f;
        else
        {
            t1 *= t1;
            n1 = t1 * t1 * grad(perm[ii + i1 + perm[jj + j1]], x1, y1);
        }

        float t2 = 0.5f - x2 * x2 - y2 * y2;
        if (t2 < 0.0f) n2 = 0.0f;
        else
        {
            t2 *= t2;
            n2 = t2 * t2 * grad(perm[ii + 1 + perm[jj + 1]], x2, y2);
        }

        // Add contributions from each corner to get the final noise value.
        // The result is scaled to return values in the interval [-1,1].
        return 40.0f * (n0 + n1 + n2); // TODO: The scale factor is preliminary!
    }


    public static float Generate(float x, float y, float z)
    {
        // Simple skewing factors for the 3D case
        const float F3 = 0.333333333f;
        const float G3 = 0.166666667f;

        float n0, n1, n2, n3; // Noise contributions from the four corners

        // Skew the input space to determine which simplex cell we're in
        float s = (x + y + z) * F3; // Very nice and simple skew factor for 3D
        float xs = x + s;
        float ys = y + s;
        float zs = z + s;
        int i = FastFloor(xs);
        int j = FastFloor(ys);
        int k = FastFloor(zs);

        float t = (float)(i + j + k) * G3;
        float X0 = i - t; // Unskew the cell origin back to (x,y,z) space
        float Y0 = j - t;
        float Z0 = k - t;
        float x0 = x - X0; // The x,y,z distances from the cell origin
        float y0 = y - Y0;
        float z0 = z - Z0;

        // For the 3D case, the simplex shape is a slightly irregular tetrahedron.
        // Determine which simplex we are in.
        int i1, j1, k1; // Offsets for second corner of simplex in (i,j,k) coords
        int i2, j2, k2; // Offsets for third corner of simplex in (i,j,k) coords

        /* This code would benefit from a backport from the GLSL version! */
        if (x0 >= y0)
        {
            if (y0 >= z0)
            { i1 = 1; j1 = 0; k1 = 0; i2 = 1; j2 = 1; k2 = 0; } // X Y Z order
            else if (x0 >= z0) { i1 = 1; j1 = 0; k1 = 0; i2 = 1; j2 = 0; k2 = 1; } // X Z Y order
            else { i1 = 0; j1 = 0; k1 = 1; i2 = 1; j2 = 0; k2 = 1; } // Z X Y order
        }
        else
        { // x0<y0
            if (y0 < z0) { i1 = 0; j1 = 0; k1 = 1; i2 = 0; j2 = 1; k2 = 1; } // Z Y X order
            else if (x0 < z0) { i1 = 0; j1 = 1; k1 = 0; i2 = 0; j2 = 1; k2 = 1; } // Y Z X order
            else { i1 = 0; j1 = 1; k1 = 0; i2 = 1; j2 = 1; k2 = 0; } // Y X Z order
        }

        // A step of (1,0,0) in (i,j,k) means a step of (1-c,-c,-c) in (x,y,z),
        // a step of (0,1,0) in (i,j,k) means a step of (-c,1-c,-c) in (x,y,z), and
        // a step of (0,0,1) in (i,j,k) means a step of (-c,-c,1-c) in (x,y,z), where
        // c = 1/6.

        float x1 = x0 - i1 + G3; // Offsets for second corner in (x,y,z) coords
        float y1 = y0 - j1 + G3;
        float z1 = z0 - k1 + G3;
        float x2 = x0 - i2 + 2.0f * G3; // Offsets for third corner in (x,y,z) coords
        float y2 = y0 - j2 + 2.0f * G3;
        float z2 = z0 - k2 + 2.0f * G3;
        float x3 = x0 - 1.0f + 3.0f * G3; // Offsets for last corner in (x,y,z) coords
        float y3 = y0 - 1.0f + 3.0f * G3;
        float z3 = z0 - 1.0f + 3.0f * G3;

        // Wrap the integer indices at 256, to avoid indexing perm[] out of bounds
        int ii = Mod(i, 256);
        int jj = Mod(j, 256);
        int kk = Mod(k, 256);

        // Calculate the contribution from the four corners
        float t0 = 0.6f - x0 * x0 - y0 * y0 - z0 * z0;
        if (t0 < 0.0f) n0 = 0.0f;
        else
        {
            t0 *= t0;
            n0 = t0 * t0 * grad(perm[ii + perm[jj + perm[kk]]], x0, y0, z0);
        }

        float t1 = 0.6f - x1 * x1 - y1 * y1 - z1 * z1;
        if (t1 < 0.0f) n1 = 0.0f;
        else
        {
            t1 *= t1;
            n1 = t1 * t1 * grad(perm[ii + i1 + perm[jj + j1 + perm[kk + k1]]], x1, y1, z1);
        }

        float t2 = 0.6f - x2 * x2 - y2 * y2 - z2 * z2;
        if (t2 < 0.0f) n2 = 0.0f;
        else
        {
            t2 *= t2;
            n2 = t2 * t2 * grad(perm[ii + i2 + perm[jj + j2 + perm[kk + k2]]], x2, y2, z2);
        }

        float t3 = 0.6f - x3 * x3 - y3 * y3 - z3 * z3;
        if (t3 < 0.0f) n3 = 0.0f;
        else
        {
            t3 *= t3;
            n3 = t3 * t3 * grad(perm[ii + 1 + perm[jj + 1 + perm[kk + 1]]], x3, y3, z3);
        }

        // Add contributions from each corner to get the final noise value.
        // The result is scaled to stay just inside [-1,1]
        return 32.0f * (n0 + n1 + n2 + n3); // TODO: The scale factor is preliminary!
    }

    public static byte[] perm = new byte[512] { 151,160,137,91,90,15,
          131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,
          190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,
          88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
          77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,
          102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196,
          135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123,
          5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
          223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9,
          129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228,
          251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107,
          49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,
          138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180,
          151,160,137,91,90,15,
          131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,
          190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,
          88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
          77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,
          102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196,
          135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123,
          5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
          223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9,
          129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228,
          251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107,
          49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,
          138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180 
        };

    private static int FastFloor(float x)
    {
        return (x > 0) ? ((int)x) : (((int)x) - 1);
    }

    private static int Mod(int x, int m)
    {
        int a = x % m;
        return a < 0 ? a + m : a;
    }

    private static float grad(int hash, float x)
    {
        int h = hash & 15;
        float grad = 1.0f + (h & 7);   // Gradient value 1.0, 2.0, ..., 8.0
        if ((h & 8) != 0) grad = -grad;         // Set a random sign for the gradient
        return (grad * x);           // Multiply the gradient with the distance
    }

    private static float grad(int hash, float x, float y)
    {
        int h = hash & 7;      // Convert low 3 bits of hash code
        float u = h < 4 ? x : y;  // into 8 simple gradient directions,
        float v = h < 4 ? y : x;  // and compute the dot product with (x,y).
        return ((h & 1) != 0 ? -u : u) + ((h & 2) != 0 ? -2.0f * v : 2.0f * v);
    }

    private static float grad(int hash, float x, float y, float z)
    {
        int h = hash & 15;     // Convert low 4 bits of hash code into 12 simple
        float u = h < 8 ? x : y; // gradient directions, and compute dot product.
        float v = h < 4 ? y : h == 12 || h == 14 ? x : z; // Fix repeats at h = 12 to 15
        return ((h & 1) != 0 ? -u : u) + ((h & 2) != 0 ? -v : v);
    }

    private static float grad(int hash, float x, float y, float z, float t)
    {
        int h = hash & 31;      // Convert low 5 bits of hash code into 32 simple
        float u = h < 24 ? x : y; // gradient directions, and compute dot product.
        float v = h < 16 ? y : z;
        float w = h < 8 ? z : t;
        return ((h & 1) != 0 ? -u : u) + ((h & 2) != 0 ? -v : v) + ((h & 4) != 0 ? -w : w);
    }
}
//  public class Noise
//  {
//      private static readonly float[] RTable = new[] // 267
//          {
//              -1.0f, 0.604974f, -0.937102f, 0.414115f, 0.576226f, -0.0161593f,
//              0.432334f, 0.103685f, 0.590539f, 0.0286412f, 0.46981f, -0.84622f,
//              -0.0734112f, -0.304097f, -0.40206f, -0.210132f, -0.919127f, 0.652033f,
//              -0.83151f, -0.183948f, -0.671107f, 0.852476f, 0.043595f, -0.404532f,
//              0.75494f, -0.335653f, 0.618433f, 0.605707f, 0.708583f, -0.477195f,
//              0.899474f, 0.490623f, 0.221729f, -0.400381f, -0.853727f, -0.932586f,
//              0.659113f, 0.961303f, 0.325948f, -0.750851f, 0.842466f, 0.734401f,
//              -0.649866f, 0.394491f, -0.466056f, -0.434073f, 0.109026f, 0.0847028f,
//              -0.738857f, 0.241505f, 0.16228f, -0.71426f, -0.883665f, -0.150408f,
//              -0.90396f, -0.686549f, -0.785214f, 0.488548f, 0.0246433f, 0.142473f,
//              -0.602136f, 0.375845f, -0.00779736f, 0.498955f, -0.268147f, 0.856382f,
//              -0.386007f, -0.596094f, -0.867735f, -0.570977f, -0.914366f, 0.28896f,
//              0.672206f, -0.233783f, 0.94815f, 0.895262f, 0.343252f, -0.173388f,
//              -0.767971f, -0.314748f, 0.824308f, -0.342092f, 0.721431f, -0.24004f,
//              -0.63653f, 0.553277f, 0.376272f, 0.158984f, -0.452659f, 0.396323f,
//              -0.420676f, -0.454154f, 0.122179f, 0.295857f, 0.0664225f, -0.202075f,
//              -0.724788f, 0.453513f, 0.224567f, -0.908812f, 0.176349f, -0.320516f,
//              -0.697139f, 0.742702f, -0.900786f, 0.471489f, -0.133532f, 0.119127f,
//              -0.889769f, -0.23183f, -0.669673f, -0.046891f, -0.803433f, -0.966735f,
//              0.475578f, -0.652644f, 0.0112459f, -0.730007f, 0.128283f, 0.145647f,
//              -0.619318f, 0.272023f, 0.392966f, 0.646418f, -0.0207675f, -0.315908f,
//              0.480797f, 0.535668f, -0.250172f, -0.83093f, -0.653773f, -0.443809f,
//              0.119982f, -0.897642f, 0.89453f, 0.165789f, 0.633875f, -0.886839f,
//              0.930877f, -0.537194f, 0.587732f, 0.722011f, -0.209461f, -0.0424659f,
//              -0.814267f, -0.919432f, 0.280262f, -0.66302f, -0.558099f, -0.537469f,
//              -0.598779f, 0.929656f, -0.170794f, -0.537163f, 0.312581f, 0.959442f,
//              0.722652f, 0.499931f, 0.175616f, -0.534874f, -0.685115f, 0.444999f,
//              0.17171f, 0.108202f, -0.768704f, -0.463828f, 0.254231f, 0.546014f,
//              0.869474f, 0.875212f, -0.944427f, 0.130724f, -0.110185f, 0.312184f,
//              -0.33138f, -0.629206f, 0.0606546f, 0.722866f, -0.0979477f, 0.821561f,
//              0.0931258f, -0.972808f, 0.0318151f, -0.867033f, -0.387228f, 0.280995f,
//              -0.218189f, -0.539178f, -0.427359f, -0.602075f, 0.311971f, 0.277974f,
//              0.773159f, 0.592493f, -0.0331884f, -0.630854f, -0.269947f, 0.339132f,
//              0.581079f, 0.209461f, -0.317433f, -0.284993f, 0.181323f, 0.341634f,
//              0.804959f, -0.229572f, -0.758907f, -0.336721f, 0.605463f, -0.991272f,
//              -0.0188754f, -0.300191f, 0.368307f, -0.176135f, -0.3832f, -0.749569f,
//              0.62356f, -0.573938f, 0.278309f, -0.971313f, 0.839994f, -0.830686f,
//              0.439078f, 0.66128f, 0.694514f, 0.0565042f, 0.54342f, -0.438804f,
//              -0.0228428f, -0.687068f, 0.857267f, 0.301991f, -0.494255f, -0.941039f,
//              0.775509f, 0.410575f, -0.362081f, -0.671534f, -0.348379f, 0.932433f,
//              0.886442f, 0.868681f, -0.225666f, -0.062211f, -0.0976425f, -0.641444f,
//              -0.848112f, 0.724697f, 0.473503f, 0.998749f, 0.174701f, 0.559625f,
//              -0.029099f, -0.337392f, -0.958129f, -0.659785f, 0.236042f, -0.246937f,
//              0.659449f, -0.027512f, 0.821897f, -0.226215f, 0.0181735f, 0.500481f,
//              -0.420127f, -0.427878f, 0.566186f
//          };

//      private const int HashTableSize = 4096; // must be a power of 2  or Hash2D and Hash1D must be modified to use % instead of &
//      private const int HashTableMask = HashTableSize - 1;
//      private readonly short[] _hashTable;

//      public Noise()
//      {
//          _hashTable = new short[HashTableSize];
//          for (short i = 0; i < HashTableSize; i++) // init
//              _hashTable[i] = i;
//          Random random = new Random();
//          for(int i = HashTableSize-1; i >=0; i--) // shuffle
//          {
//              int j = random.Next(HashTableSize);
//              short temp = _hashTable[i];
//              _hashTable[i] = _hashTable[j];
//              _hashTable[j] = temp;
//          }
//      }

//      private short Hash2D(int a, int b)
//      {
//          return _hashTable[_hashTable[(a & HashTableMask)] ^ (b & HashTableMask)];
//      }

//      private short Hash1D(int a, int b)
//      {
//          return _hashTable[(a ^ (b & HashTableMask)) & HashTableMask];
//      }

//      private double SCurve(double t)
//      {
//          return t*t*(3 - 2*t);
//      }

////      #define INCRSUMP(mp,s,x,y,z) \
////((s)*((mp[0])*0.5f + (mp[1])*(x) + (mp[2])*(y) + (mp[3])*(z)))
//  }
