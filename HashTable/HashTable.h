#include <vector>
#include <list>

namespace DataStructure {
    // to compute ASL, i decide to make _reserved immutable
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

    class HashTableChaining: public HashTableBase {
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
