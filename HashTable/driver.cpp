#include "HashTable.h"
#include <numeric>
#include <cstdio>
#include <iostream>
#include <fstream>

const char *in_file = "..\\HashTable\\test.txt";

using namespace std;
using namespace DataStructure;

void hash_test(HashTableBase &ht, const int *keys, size_t key_num, size_t reserve)
{
    double failed_asl, asl;
    int failed_sl, sl;
    for (size_t i = 0; i < key_num; i++)
    {
        ht.insert(keys[i]);
    }
    asl = ht.getAsl();
    int total_failed(0);

    ht.dump_keys();
    printf("\n");

    printf("%-10s:", "keys");
    for (size_t i = 0; i < key_num; i++)
    {
        printf(" %5d", keys[i]);
    }
    printf("\n%-10s:", "SL");
    for (size_t i = 0; i < key_num; i++)
    {
        sl = ht.find(keys[i]);
        printf(" %5d", sl);
    }
    printf("\n%-10s:", "Failed SL");
    for (size_t i = 0; i < reserve; i++)
    {
        failed_sl = ht.assume_notin_find(i);
        total_failed += failed_sl;
        printf(" %5d", failed_sl);
    }
    failed_asl = (double)total_failed / reserve;
    printf("\nASL: %lf", asl);
    printf("\nFailed ASL: %lf\n", failed_asl);
}

int main()
{
    int key_num, reserve;
    ifstream in(in_file);
    in >> key_num;

    int *keys = new int[key_num];

    for (int i = 0; i < key_num; i++)
        in >> keys[i];
    in >> reserve;
    HashTableChaining htc(reserve, reserve);
    printf("=====================================CHAINING==================================\n");
    hash_test(htc, keys, key_num, reserve);

    printf("\n");
    printf("======================================PROBING==================================\n");
    HashTableProbing htp(reserve, reserve);
    hash_test(htp, keys, key_num, reserve);
    
    delete[] keys;
    getchar();
    return 0;
}