using BCrypt.Net;

namespace WolfInvoice.Converters;

/// <summary>
/// Provides methods for converting between work factor and hash type values.
/// </summary>
public class HashTypeConverter
{
    /// <summary>
    /// Converts the specified work factor value to the corresponding BCrypt HashType.
    /// </summary>
    /// <param name="workFactor">The work factor value to be converted.</param>
    /// <returns>The corresponding BCrypt HashType.Returns <see cref="HashType.SHA384"/> with invalid value.</returns>
    public HashType GetHashType(int workFactor)
    {
        return workFactor switch
        {
            var wf when (wf < 4 || wf > 31) => HashType.SHA384,
            var wf when (wf < 6) => HashType.SHA256,
            var wf when (wf < 8) => HashType.SHA384,
            _ => HashType.SHA512,
        };
    }

    /// <summary>
    /// Converts the specified BCrypt HashType to the corresponding work factor value.
    /// </summary>
    /// <param name="hashType">The BCrypt HashType to be converted.</param>
    /// <returns>The corresponding work factor value. Returns <code>7</code> with invalid value</returns>
    public int GetWorkFactor(HashType hashType)
    {
        return hashType switch
        {
            HashType.SHA256 => 5,
            HashType.SHA384 => 7,
            HashType.SHA512 => 10,
            HashType.None => 7,
            _ => 7,
        };
    }
}
