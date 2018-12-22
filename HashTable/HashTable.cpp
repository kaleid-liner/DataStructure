#include "HashTable.h"
#include <exception>
#include <algorithm>
#include <cstdio>
#include <vector>
#include <numeric>
using namespace std;

namespace {
    size_t findLargestPrimeNumber(size_t n) 
    {
        if (n < 2) return 1;
        bool *isprime = new bool[n + 1];
        fill(isprime, isprime + n + 1, true);
        for (size_t i = 2; i < n; i++)
        {
            if (isprime[i])
            {
                for (size_t j = 2 * i; j < n; j += i)
                    isprime[j] = false;
            }
        }
        int ret = 2;
        for (size_t i = n; i > 0; i--)
        {
            if (isprime[i])
            {
                ret = i;
                break;
            }
        }
        delete[] isprime;
        return ret;
    }
}

namespace DataStructure {
    HashTableBase::HashTableBase(size_t n) : 
        HashTableBase(n, findLargestPrimeNumber(n)){ }

    HashTableBase::HashTableBase(size_t n, size_t p) : _total_sl(0), _key_num(0),
        _mod(p), _reserved(n) { }

    void HashTableChaining::insert(int key)
    {
        int hashval = hashing(key);
        _keys_list[hashval].push_back(key);
        _total_sl += _keys_list[hashval].size();
        _key_num++;
    }

    int HashTableChaining::find(int key)
    {
        int hashval = hashing(key);
        int search_length(1);
        bool found(false);
        for (auto _key : _keys_list[hashval])
        {
            if (_key == key)
            {
                found = true;
                break;
            }
            search_length++;
        }
        if (found) return search_length;
        else return -1;
    }

    int HashTableChaining::assume_notin_find(int key)
    {
        int hashval = hashing(key);
        return _keys_list[hashval].size() + 1;
    }

    void HashTableChaining::dump_keys()
    {
        printf("%-10s:", "address");
        for (size_t i = 0; i < _reserved; i++)
            printf(" %5u", i);
        printf("\n%-10s:", "keys");
        int i = 0;
        bool end = false;
        vector<list<int>::const_iterator> iters;
        for (size_t i = 0; i < _reserved; i++)
            iters.push_back(_keys_list[i].cbegin());
        while(true)
        {
            end = true;
            for (size_t j = 0; j < _reserved; j++)
            {
                if (iters[j] != _keys_list[j].cend())
                {
                    printf(" %5d", *iters[j]);
                    end = false;
                    iters[j]++;
                }
                else printf("     -");
            }
            if (end) break;
            printf("\n%-10s ", " ");
            i++;
        }
    }

    void HashTableProbing::insert(int key)
    {
        if (_key_num == _reserved)
            throw out_of_range("keys out of reserved space");
        int hashval = hashing(key);
        int find_space;
        for (find_space = hashval; _used[find_space]; 
            find_space = (find_space + 1) % _reserved)
        {
            _total_sl++;
        }
        _used[find_space] = true;
        _keys[find_space] = key;
        _total_sl++;
        _key_num++;
    }

    // return -1 if not found, else return search length
    int HashTableProbing::find(int key)
    {
        int hashval = hashing(key);
        if (_keys[hashval] == key)
            return 1;
        int search_length(1);
        int find_space = (hashval + 1) % _reserved;

        while (_used[find_space] && 
            _keys[find_space] != key && 
            find_space != hashval)
        {
            find_space = (find_space + 1) % _reserved;
            search_length++;
        }
        if (!_used[find_space]) return -1;
        if (_keys[find_space] != key) return -1;
        return search_length;
    }

    int HashTableProbing::assume_notin_find(int key)
    {
        int hashval = hashing(key);
        int search_length(1);
        for (int i = hashval; _used[i]; i = (i + 1) % _reserved)
        {
            search_length++;
        }
        return search_length;
    }

    void HashTableProbing::dump_keys()
    {
        printf("%-10s:", "address");
        for (size_t i = 0; i < _reserved; i++)
            printf(" %5u", i);
        printf("\n%-10s:", "keys");
        for (size_t i = 0; i < _reserved; i++)
        {
            if (_used[i])
            {
                printf(" %5d", _keys[i]);
            }
            else printf("     -");
        }
    }
}
