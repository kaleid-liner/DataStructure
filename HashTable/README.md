# HashTable实验报告

**姓名**：魏剑宇
**学号**：PB17111586

---

## 实验要求

1. 输入关键字序列；
2. 用除留余数法构建哈希函数，用线性探测法解决冲突，构建哈希表HT1；
3. 用除留余数法构建哈希函数，用拉链法解决冲突，构建哈希表HT2；
4. 分别对HT1和HT2计算在等概率情况下查找成功和查找失败的ASL；
5. 分别在HT1和HT2中查找给定的关键字，给出比较次数

## 实验内容

**实验题目**：HashTable

**内容描述**：

1. 设计好HashTable的接口
2. 实现接口
3. 在给定测试集上进行测试

## 关键代码描述

代码通过C++实现。代码位于[我的数据结构Github Repo](https://github.com/kaleid-liner/DataStructure/tree/master/HashTable).

实现了三个HashTable类

- 抽象基类`HashTableBase`，实现了一些基础的方法，提供接口
- `HashTableChaining`，继承`HashTableBase`，使用链表法解决冲突
- `HashTableProbing`，继承`HashTableBase`，使用线性探测法解决冲突

这些HashTable有公共的接口，提供的方法有

- Strategy getHandlingStrategy()，得到此类处理冲突的策略。其中，Strategy定义如下

  ```cpp
   enum class Strategy { linear_probing, chaining };
  ```

- void insert(int key)，插入新的key值

- int find(int key)，寻找某一key值，若找到，则返回查找长度；否则，返回-1

- int assume_notin_find(int key)，假设某一key值不在表中，返回查找长度。用于计算失败查找次数

- void dump_keys()，输出表的结构

### 抽象基类定义

```cpp
// HashTable.h 

class HashTableBase {
    public:
        enum class Strategy { linear_probing, chaining };

        HashTableBase(size_t n);
        HashTableBase(size_t n, size_t p);
        virtual Strategy getHandlingStrategy() = 0;
        virtual void insert(int key) = 0;
        virtual int find(int key) = 0;
        virtual int assume_notin_find(int key) = 0;
        virtual void dump_keys() = 0;

        double getAsl() { return (double)_total_sl / _key_num; }
        size_t size() { return _key_num; }

    protected:
        size_t hashing(int key)
        {
            return ((key % (long long)_mod) + _mod) % _mod;
        }
        const size_t _mod;
        size_t _total_sl;
        size_t _key_num;
        const size_t _reserved;

    private:

};
```

如上，给出了一些定义，并提供了HashTable中使用的hash函数hashing。hashing函数中为了防止负值的出现写成`((key % (long long)_mod) + _mod) % _mod`，使用long long是因为size_t（unsigned）并不一定能在int中存下。

其构造函数如下

```cpp
// HashTable.cpp

HashTableBase::HashTableBase(size_t n) : 
    HashTableBase(n, findLargestPrimeNumber(n)){ }

HashTableBase::HashTableBase(size_t n, size_t p) : _total_sl(0), _key_num(0),
    _mod(p), _reserved(n) { }
```

提供两个构造函数，一个是只接受一个参数n，p通过找n以下的最大质数来确定，并委托第二个来构造。另一个直接提供n和p。第一个构造函数在本次实验中没有用到。

### 子类定义

```cpp
class HashTableProbing : public HashTableBase {
    public:
        HashTableProbing(size_t n) : HashTableBase(n), _used(n, false), _keys(n) { }
        HashTableProbing(size_t n, size_t p) : HashTableBase(n, p), _used(n, false), _keys(n) { }
        Strategy getHandlingStrategy() override { return Strategy::linear_probing; }
        void insert(int key) override;
        int find(int key) override;
        int assume_notin_find(int key) override;
        void dump_keys() override;

    private:
        std::vector<int> _keys;
        std::vector<bool> _used;
    };

class HashTableChaining : public HashTableBase {
    public:
        HashTableChaining(size_t n) : HashTableBase(n), _keys_list(n) { }
        HashTableChaining(size_t n, size_t p) : HashTableBase(n, p), _keys_list(n) { }
        Strategy getHandlingStrategy() override { return Strategy::chaining; }
        void insert(int key) override;
        int find(int key) override;
        int assume_notin_find(int key) override;
        void dump_keys() override;

    private:
        std::vector<std::list<int>> _keys_list;

    };
}
```

继承自HashTableBase，并增加了一些存储key值的私有域。

由于各个方法的实现逻辑非常简单，这里只举一个进行说明

```cpp
//HashTable.cpp

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
```

这里，如果所有空间已经用完，将抛出一个异常。如果还有剩余的空间，得到hash值后，线性的查找空余的位置，之后插入，并增加_total_sl，之后将该位置标记为已使用。

### main函数

有了这些，main函数就十分简单了。

这里是其中的一部分代码

```cpp
int key_num, reserve;
int *keys = new int[key_num];

// deal with input

printf("=====================================CHAINING==================================\n");
HashTableChaining htc(reserve, reserve);
hash_test(htc, keys, key_num, reserve);

printf("\n");
printf("======================================PROBING==================================\n");
HashTableProbing htp(reserve, reserve);
hash_test(htp, keys, key_num, reserve);
```

在处理完输入后，从keys构造对应的hash表，并进行测试。由于良好的设计模式和一致的接口，代码没有重复。

hash_test的定义如下

```cpp
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
```

通过c++的动态类型和virtual函数的动态特性，能将此test函数用于所有继承自HashTableBase的类。

## 总结

- 这次的实验相对比较简单，算法也十分朴素。
- 使用C++ OOP能写出简洁优雅可读性高的代码，并避免代码重复。