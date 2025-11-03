# Blockchain Project Overview

This project implements a simplified blockchain in C# (.NET 8), using Proof-of-Work (PoW) and basic blockchain operations such as block creation, mining, and transaction handling.

---

## Key Features

- **User Generation**: 1000 users with random balances and public keys.
- **Transaction Generation**: 10,000 transactions between users.
- **Block Creation**: Blocks are created with 100 transactions at a time.
- **Proof-of-Work (PoW)**: Blocks are mined by solving a cryptographic puzzle (difficulty = 3, meaning the hash starts with 3 zeros).
- **Blockchain Rendering**: After mining, the blockchain is displayed as an HTML page.

---

## Workflow

1. **Program Startup**: 
   - Generates 1000 users and 10,000 transactions.
   - Groups transactions into blocks of 100.

2. **Block Mining**: 
   - For each block, a hash is calculated and the Proof-of-Work puzzle is solved by adjusting a `nonce` until a valid hash (with 3 leading zeros) is found.

3. **Block Addition**:
   - Once a valid hash is found, the block is added to the blockchain and user balances are updated.
   
4. **HTML Output**:
   - After all blocks are mined, the blockchain is saved as an HTML file and automatically opened in your browser.

---

## Project Structure

- **Main Files**:
  - `Program.cs`: Entry point for the application.
  - `BlockchainService.cs`: Core logic for blockchain operations (e.g., block mining, transaction handling).
  - `CustomHasher.cs`: Custom hashing function for mining.
  - `Models/`: Contains data models like `Block`, `Transaction`, `User`.
  - `Services/`: Helper services like rendering the blockchain and generating data.

---

## How to Run

1. Open a terminal and navigate to the project folder:

```bash
cd C:\path\to\BlockChain
````

2. Build and run the project:

```bash
dotnet build
dotnet run
```

The program will generate users, transactions, mine blocks, and create an `Output/blockchain.html` file which will open automatically in your browser.

---

## Customization

By default, the project generates:

* 1000 users
* 10,000 transactions
* Blocks with 100 transactions
* Difficulty level of 3 (hash starts with 3 zeros)

You can modify these settings in `BlockchainService.cs` or add custom configuration options via the command line if you prefer.

---

## Output

* **HTML Visualization**: After mining, an HTML file is created (`Output/blockchain.html`), which visually represents the entire blockchain.

* **Block Information**: Each block’s details are printed in the console as it is mined. You can also inspect each block's transactions directly in the `blockchain.html` output.

---

## Viewing Block Transactions

* Console logs show a summary of each block during mining.
* A detailed view of each block’s transactions is available in the generated `blockchain.html` file.
---