#include "polynomial.h"
using namespace std;

namespace DataStructure{
    Polynomial::Polynomial(const Polynomial &rhs) : root(new PolyNode)
    {
        deepcopy(root->next, rhs.root->next);
    }

    Polynomial operator+(const Polynomial &lhs, const Polynomial &rhs)
    {
        Polynomial newPoly(lhs);
        newPoly += rhs;
        return newPoly;
    }

    Polynomial &Polynomial::operator+=(const Polynomial &rhs)
    {
        auto curr = root, r = rhs.root->next;
        PolyNode *prev;
        while (curr->next && r)
        {
            prev = curr;
            curr = curr->next;
            if (curr->expo > r->expo)
                continue;
            else if (curr->expo == r->expo)
            {
                curr->coeff += r->coeff;
                r = r->next;
            }
            else{
                prev->next = new PolyNode();
                prev->next->coeff = r->coeff;
                prev->next->expo = r->expo;
                prev->next->next = curr;
                r = r->next;
            }
        }
        return *this;
    }

    Polynomial operator-(const Polynomial &rhs)
    {
        Polynomial newPoly;
        auto curr = newPoly.root;
        auto r = rhs.root->next;
        while (r)
        {
            curr->next = new Polynomial::PolyNode;
            curr = curr->next;
            curr->coeff = -r->coeff;
            curr->expo = -r->expo;
        }
        return newPoly;
    }

    Polynomial operator-(const Polynomial &lhs, const Polynomial &rhs)
    {
        Polynomial newPoly(lhs);
        newPoly -= rhs;
        return newPoly;
    }

    Polynomial &Polynomial::operator-=(const Polynomial &rhs)
    {
        (*this) += (-rhs);
        return *this;
    }

    Polynomial operator*(const Polynomial &lhs, const Polynomial &rhs)
    {
        Polynomial result;
        auto r = rhs.root->next;
        while(r)
        {
            result += lhs.dot(r->coeff, r->expo);
        }
        return result;
    }

    Polynomial &Polynomial::operator*=(const Polynomial &rhs)
    {
        return *this = *this * rhs;
    }

    Polynomial Polynomial::dot(double coeff, int expo) const 
    {
        Polynomial result;
        deepcopy(result.root->next, root->next);
        auto curr = result.root->next;
        while (curr)
        {
            curr->coeff *= coeff;
            curr->expo += expo;
        }
        return result;
    }

    void Polynomial::deepcopy(PolyNode *&dest, const PolyNode *src)
    {
        if (src == nullptr)
            return;
        if (dest == nullptr)
            dest = new PolyNode();
        dest->coeff = src->coeff;
        dest->expo = src->expo;
        deepcopy(dest->next, src->next);
    }

    void Polynomial::destruct(PolyNode *dest)
    {
        if (dest == nullptr)
            return;
        if (dest->next)
            destruct(dest->next);
        delete dest;
    }

    void swap(Polynomial &lhs, Polynomial &rhs)
    {
        std::swap(lhs.root->next, rhs.root->next);
    }
}