using System;

/// <summary>
/// Sandart complecs number
/// </summary>
public class Complex
{
    /// <summary>
    /// Real part of complex number.
    /// </summary>
    float x;

    /// <summary>
    /// Imaginary part of complex number.
    /// </summary>
    float y;

    /// <summary>
    /// Get or set the real part of complex number.
    /// </summary>
    /// <value> Return float number with real part. </value>
    public float Real 
    { 
        get { return x; } 
        set { x = value; }
    }

    /// <summary>
    /// Gets or sets the imaginary part of number.
    /// </summary>
    /// <value> Return float number with imaginary part.</value>
    public float Imaginary 
    { 
        get { return y; } 
        set { y = value; }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Complex"/> class.
    /// Set real and imaginary parts to 0.
    /// </summary>
    public Complex()
    {
        x=0;
        y=0;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Complex"/> class.
    /// Set real and imaginary parts to X and Y respectively.
    /// </summary>
    /// <param name="X">Real part.</param>
    /// <param name="Y">Imaginary part.</param>
    public Complex(float X,float Y)
    {
        x = X;
        y = Y;
    }

    /// <summary>
    /// Returns a <see cref="System.String"/> that represents the current <see cref="Complex"/>.
    /// </summary>
    /// <returns>A <see cref="System.String"/> that represents the current <see cref="Complex"/>.</returns>
    public override string ToString()
    {
        return string.Format("({0},{1})",x,y);
    }

    /// <param name="c1"> First complex number. </param>
    /// <param name="c2"> Second complex number.</param>
    public static Complex operator +(Complex c1, Complex c2) 
    {
        return new Complex(c1.x + c2.x, c1.y + c2.y);
    }

    /// <param name="c1"> First complex number. </param>
    /// <param name="c2"> Second complex number.</param>
    public static Complex operator -(Complex c1, Complex c2) 
    {
        return new Complex(c1.x - c2.x, c1.y - c2.y);
    }

    /// <param name="c1"> First complex number. </param>
    /// <param name="c2"> Second complex number.</param>
    public static Complex operator *(Complex c1, Complex c2)
    {
        return new Complex(c1.x * c2.x - c1.y * c2.y, c1.x * c2.y + c1.y * c2.x);
    }

    /// <param name="c1"> First complex number. </param>
    /// <param name="c2"> Number with floating point.</param>
    public static Complex operator *(Complex c1, float c2)
    {
        return new Complex(c1.x * c2, c1.y * c2);
    }

    /// <param name="c1"> First complex number. </param>
    /// <param name="c2"> Number with floating point.</param>
    public static Complex operator /(Complex c1, float c2)
    {
        return new Complex(c1.x / c2, c1.y / c2);
    }

    /// <summary>
    /// Scalar product of two complex numbers.
    /// </summary>
    /// <returns> Return complex number with scalar prod..</returns>
    /// <param name="c1"> First complex number. </param>
    /// <param name="c2"> Second complex number.</param>
    public static Complex ScalarProd(Complex c1, Complex c2)
    {
        return new Complex(c1.x * c2.x + c1.y * c2.y, c1.x * c2.y - c1.y * c2.x);
    }

    /// <param name="c1"> First complex number. </param>
    /// <param name="c2"> Second complex number.</param>
    public static bool operator ==(Complex c1, Complex c2)
    {
        return (c1.x==c2.x) && (c1.y==c2.y);
    }

    /// <param name="c1"> First complex number. </param>
    /// <param name="c2"> Second complex number.</param>
    public static bool operator !=(Complex c1, Complex c2)
    {
        return !((c1.x==c2.x) && (c1.y==c2.y));
    }

    /// <summary>
    /// Determines whether the specified <see cref="System.Object"/> is equal to the current <see cref="Complex"/>.
    /// </summary>
    /// <param name="obj">The <see cref="System.Object"/> to compare with the current <see cref="Complex"/>.</param>
    /// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to the current <see cref="Complex"/>;
    /// otherwise, <c>false</c>.</returns>
    public override bool Equals(object obj)
    {
        return this == (Complex)obj;
    }

    /// <summary>
    /// Serves as a hash function for a <see cref="Complex"/> object.
    /// </summary>
    /// <returns>A hash code for this instance that is suitable for use in hashing algorithms and data structures such as a hash table.</returns>
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
