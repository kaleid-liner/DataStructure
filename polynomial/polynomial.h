#include <iostream>

namespace DataStructure {

    class Polynomial {
        friend Polynomial operator+(const Polynomial &lhs, const Polynomial &rhs);
        friend Polynomial operator-(const Polynomial &lhs, const Polynomial &rhs);
        friend Polynomial operator-(const Polynomial &rhs);
        friend Polynomial operator*(const Polynomial &lhs, const Polynomial &rhs);
        friend void swap(Polynomial &lhs, Polynomial &rhs);
    public:
        Polynomial &operator+=(const Polynomial &rhs);
        Polynomial &operator-=(const Polynomial &lhs);
        Polynomial &operator*=(const Polynomial &lhs);
        Polynomial() : root(new PolyNode) {}
        Polynomial(const Polynomial &rhs);
        Polynomial(Polynomial &&rhs) : root(new PolyNode){
            root->next = rhs.root->next;
            rhs.root->next = nullptr;
        }
        ~Polynomial() { destruct(root); }
        Polynomial &operator=(Polynomial rhs){
            swap(*this, rhs);
            return *this;
        }
        Polynomial dot(double coeff, int expo) const;

    private:
        struct PolyNode{
            double coeff;
            int expo;
            PolyNode *next;
            PolyNode() : next(nullptr) { }
        };

        PolyNode *root;

        void static deepcopy(PolyNode *&dest, const PolyNode *src);
        void static destruct(PolyNode *dest);

    };

    Polynomial operator+(const Polynomial &lhs, const Polynomial &rhs);
    Polynomial operator-(const Polynomial &lhs, const Polynomial &rhs);
    Polynomial operator-(const Polynomial &rhs);
    Polynomial operator*(const Polynomial &lhs, const Polynomial &rhs);
    void swap(Polynomial &lhs, Polynomial &rhs);
}