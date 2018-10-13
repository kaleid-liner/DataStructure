#pragma once
#ifndef POLYNOMIAL_H
#define POLYNOMIAL_H

#include <iostream>
#include <string>

namespace DataStructure {

	class Polynomial {
		friend Polynomial operator+(const Polynomial &lhs, const Polynomial &rhs);
		friend Polynomial operator-(const Polynomial &lhs, const Polynomial &rhs);
		friend Polynomial operator-(const Polynomial &rhs);
		friend Polynomial operator*(const Polynomial &lhs, const Polynomial &rhs);
		friend Polynomial operator/(const Polynomial &lhs, const Polynomial &rhs);
		friend void swap(Polynomial &lhs, Polynomial &rhs);
	public:
		Polynomial & operator+=(const Polynomial &rhs);
		Polynomial &operator-=(const Polynomial &rhs);
		Polynomial &operator*=(const Polynomial &rhs);
		Polynomial &operator/=(const Polynomial &rhs);
		Polynomial() : root(new PolyNode) {
			//root->expo represents the length of the polynomial
			root->expo = 0;
		}
		Polynomial(const Polynomial &rhs);
		Polynomial(Polynomial &&rhs) : root(new PolyNode) {
			root->next = rhs.root->next;
			root->expo = rhs.root->expo;
			rhs.root->next = nullptr;
		}
		~Polynomial() { destruct(root); }
		Polynomial &operator=(const Polynomial &rhs);
		Polynomial &operator=(Polynomial &&rhs) noexcept;
		Polynomial dot(double coeff, int expo) const;
		size_t length() const { return root->expo; }

		//evaluate the polynomial 
		double eval(double x) const;
		std::string toString() const;

		//exception : throw logic_error if expression illegal
		static Polynomial parse(const std::string &expr);

	private:
		struct PolyNode {
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
	Polynomial operator-(const Polynomial &opnd);
	Polynomial operator*(const Polynomial &lhs, const Polynomial &rhs);
	Polynomial operator/(const Polynomial &lhs, const Polynomial &rhs);
	std::ostream &operator<<(std::ostream &os, const Polynomial &rhs);
	std::istream &operator>>(std::istream &is, Polynomial &rhs);
	void swap(Polynomial &lhs, Polynomial &rhs);
}

#endif