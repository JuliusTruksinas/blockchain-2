using System.Text;

namespace BlockChain.Hashers;

public class CustomHasher : IHasher
{
    private const ulong START_CONST1 = 0x243F6A8885A308D3;
    private const ulong START_CONST2 = 0x13198A2E03707344;
    private const ulong START_CONST3 = 0xA4093822299F31D0;
    private const ulong START_CONST4 = 0x082EFA98EC4E6C89;

    public string Hash(string input)
    {
        ulong state1 = START_CONST1;
        ulong state2 = START_CONST2;
        ulong state3 = START_CONST3;
        ulong state4 = START_CONST4;

        var bytes = Encoding.UTF8.GetBytes(input);

        AbsorbData(bytes, ref state1, ref state2, ref state3, ref state4);
        MixStates(ref state1, ref state2, ref state3, ref state4);

        return ConvertToHexString(ref state1, ref state2, ref state3, ref state4);
    }

    private static void AbsorbData(
    byte[] data,
    ref ulong state1,
    ref ulong state2,
    ref ulong state3,
    ref ulong state4)
    {
        foreach (byte b in data)
        {
            state1 = RotateLeft(state1 + b, 16) ^ state2;
            state2 = RotateLeft(state2 ^ b, 13) + state3;
            state3 = RotateLeft(state3 + b, 25) ^ state4;
            state4 = RotateLeft(state4 ^ b, 31) + state1;
        }
    }

    static void MixStates(
        ref ulong state1,
        ref ulong state2,
        ref ulong state3,
        ref ulong state4,
        int rounds = 4)
    {
        for (int i = 0; i < rounds; i++)
        {
            state1 = RotateLeft(state1 + state4, 11) ^ state2;
            state2 = RotateLeft(state2 + state1, 35) ^ state3;
            state3 = RotateLeft(state3 + state2, 36) ^ state4;
            state4 = RotateLeft(state4 + state3, 21) ^ state1;
        }
    }

    private static ulong RotateLeft(ulong x, int n)
    {
        return (x << n) | (x >> (64 - n));
    }

    private static string ConvertToHexString(
        ref ulong state1,
        ref ulong state2,
        ref ulong state3,
        ref ulong state4
    )
    {
        return string.Concat(
            state1.ToString("X16"),
            state2.ToString("X16"),
            state3.ToString("X16"),
            state4.ToString("X16")
        );
    }
}