using UnityEngine;
using System;

/// <summary>
/// Class that provides functions which can work with contour
/// </summary>
public class Contour
{
    /// <summary>
    /// Complex numbers which describe contour
    /// </summary>
    private Complex[] Points;

    /// <summary>
    /// Gets or sets the complex numbers with vectors of contour.
    /// </summary>
    /// <value> The complex numbers.</value>
    public Complex[] Numbers 
    {
        get { return Points; } 
        set { Points = value; } 
    }

    /// <summary>
    /// Gets the length of contour (size of array with complex numbers).
    /// </summary>
    /// <value> The length of contour. </value>
    public int Length { get { return Numbers.Length; } }

    /// <summary>
    /// Gets the magnitude of current contour.
    /// </summary>
    /// <value> The float number with magnitude of current contour. </value>
    public float Magnitude
    {
        get
        {
            float sum = 0;
            foreach (Complex c in Numbers)
            {
                float num = Mathf.Sqrt(c.Real * c.Real + c.Imaginary * c.Imaginary);
                sum += num * num;
            }
            return Mathf.Pow(sum, 0.5f);
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Contour"/> class.
    /// </summary>
    public Contour()
    {
        Numbers = new Complex[0];
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Contour"/> class via copying data from old contour.
    /// </summary>
    /// <param name="original"> Original contour. </param>
    public Contour(Contour original)
    {
        Numbers = new Complex[original.Length];
        for (int i = 0; i < original.Length; i++)
            Numbers[i] = original.Numbers[i];
    }

    /// <summary>
    /// Adds the complex number (vector) to contour.
    /// </summary>
    /// <param name="c"> Complex number with vector data. </param>
    public void AddNumber(Complex c)
    {
        Array.Resize<Complex>(ref Points, Length + 1);
        Numbers[Length - 1] = c;
    }

    /// <summary>
    /// Smooth the current contour to new size.
    /// </summary>
    /// <param name="newLen"> New length. </param>
    public void Smooth(int newLen)
    {
        if (Numbers.Length == 0)
            return;

        Complex[] points = new Complex[newLen];
        int i;

        if (newLen < Numbers.Length)
        {
            for (i = 0; i < newLen; i++)
                points[i] = new Complex();
            for (i = 0; i < Numbers.Length; i++)
                points[i * newLen / Numbers.Length] += Numbers[i];
            Numbers = points;
        }

        if (newLen > Numbers.Length)
        {
            for (i = 0; i < newLen; i++)
            {
                float index = 1f * i * (Numbers.Length-1) / newLen;
                int j = (int)index;
                float k = index - j;
                points[i] = Numbers[j] * (1 - k) + Numbers[j + 1] * k;
            }
            Numbers = points;
        }
    }

    /// <summary>
    /// Loads contour data from screen gesture. (see this class at VallVerk's SAG project, for example)
    /// </summary>
    /// <param name="g"> Gesture with screen-touch data. </param>
    public void LoadFromGesture(Gesture g)
    {
        Numbers = new Complex[g.FramesCount];
        int i;
        for (i = 0; i < Numbers.Length; i++)
            Numbers[i] = new Complex(g.Frames[i].deltaPosition.x, g.Frames[i].deltaPosition.y);
    }

    /// <summary>
    /// Push vectors of current contour to forward.
    /// </summary>
    public void Push()
    {
        if (Numbers.Length == 0)
            return;

        Complex buf = Numbers[0];
        for (int i = 1; i < Numbers.Length; i++)
            Numbers[i - 1] = Numbers[i];
        Numbers[Numbers.Length - 1] = buf;
    }

    /// <summary>
    /// Reverse vectors array.
    /// </summary>
    public void Reverse()
    {
        Array.Reverse(Numbers);
    }

    /// <summary>
    /// Scalars product of two contours.
    /// WARNING: contours need have same lengths!
    /// </summary>
    /// <returns> The complex number with scalar product of two contours. </returns>
    /// <param name="c1"> First contour. </param>
    /// <param name="c2"> Second contour. </param>
    public static Complex ScalarProd(Contour c1,Contour c2)
    {
        if (c1.Length != c2.Length || c1.Length == 0 || c2.Length == 0)
            return new Complex();

        Complex res = Complex.ScalarProd(c1.Numbers[0], c2.Numbers[0]);
        for (int i=1;i<c1.Length;i++)
            res+=Complex.ScalarProd(c1.Numbers[i], c2.Numbers[i]);

        return res;
    }

    /// <summary>
    /// Normalized scalar product of two contoures.
    /// The real part of number means level of similarity of two contoures (value between 0 and 1).
    /// The imaginary part means angle of rotation second contour about first.
    /// WARNING: same contoures with different order of vectors was different contours.
    /// </summary>
    /// <returns> Complex number with scalar prod.. </returns>
    /// <param name="c1"> First contour. </param>
    /// <param name="c2"> Second contour. </param>
    public static Complex NormalScalarProd(Contour c1,Contour c2)
    {
        return ScalarProd(c1, c2) / (c1.Magnitude * c2.Magnitude);
    }

    /// <summary>
    /// Autocorrelation function, which retun array with Complex numbers of normal products of
    /// all orders of vectors in both contours. Needs to correct NormalScalarProd funtion warning.
    /// </summary>
    /// <returns>The func.</returns>
    /// <param name="c1"> First contour. </param>
    /// <param name="c2"> Second contour. </param>
    public static Complex[] InterrelationFunc(Contour c1,Contour c2)
    {
        if (c1.Length != c2.Length)
            return null;

        int len = c2.Length ;
        Complex[] res = new Complex[len];
        for (int i = 0; i < len; i++)
        {
            res[i] = NormalScalarProd(c1, c2);
            c2.Push();
        }
        return res;
    }

    /// <summary>
    /// Autocorrelation function, which return comple number with data of comparing of two contours.
    /// The real part of number means level of similarity of two contoures (value between 0 and 1).
    /// The imaginary part means angle of rotation second contour about first.
    /// </summary>
    /// <returns> Complex number. </returns>
    /// <param name="c1"> First contour. </param>
    /// <param name="c2"> Second contour. </param>
    public static Complex MaxIterrelationFunc(Contour c1,Contour c2)
    {
        Complex[] IF = InterrelationFunc(c1, c2);
        Complex max = IF[0];
        for (int i=1;i<IF.Length;i++)
            if (IF[i].Real > max.Real)
                max = IF[i];
        return max;
    }

    /// <summary>
    /// Return new contour with rectangle form.
    /// </summary>
    public static Contour Rectangle()
    {
        Contour a = new Contour();
        int i;
        for (i = 0; i < 5; i++)
            a.AddNumber(new Complex(1, 0));
        for (i = 0; i < 5; i++)
            a.AddNumber(new Complex(0, -1));
        for (i = 0; i < 5; i++)
            a.AddNumber(new Complex(-1, 0));
        for (i = 0; i < 5; i++)
            a.AddNumber(new Complex(0, 1));
        return a;
    }

    /// <summary>
    /// Return new contour with circle form.
    /// </summary>
    public static Contour Circle()
    {
        Contour a = new Contour();
        for (float i = 0; i < 3.1415f * 2; i += 0.2f)
            a.AddNumber(new Complex(Mathf.Cos(i),Mathf.Sin(i)));
        return a;
    }
}