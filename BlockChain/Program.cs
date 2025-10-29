using BlockChain.Hashers;
using BlockChain.Services;

namespace BlockChain;

class Program
{
    static void Main(string[] args)
    {
        var hasher = new CustomHasher();
        var blockchainService = new BlockchainService(hasher);

        blockchainService.Run();
    }
}
