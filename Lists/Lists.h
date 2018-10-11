#pragma once
#include <iostream>
#include <string>
#include <memory>
#include <functional>
#include <sstream>
#include <exception>
#include <stack>
#include <map>

namespace DataStructures {
	template <typename T> struct _ListsNode;

	template <typename T>
	/*
		where T supports operator<< and operator>>,
	*/
	class Lists {
	public:
		Lists() = delete;
		explicit Lists(const std::string&);
		Lists(Lists &&) noexcept;
		bool empty() const {
			return root == nullptr;
		}
		std::string to_string() const;
		void for_each(const std::function<void(const T&)>&) const;
		void for_each(const std::function<void(T &)>&);
		static Lists<T> parse(const std::string&);

	private:
		std::shared_ptr<_ListsNode<T>> root;
		static void create_lists(std::istream &, std::shared_ptr<_ListsNode<T>> &);
		static void traverse(_ListsNode<T> &, const std::function<void(T &)>&);
		static void to_string(std::ostream &, const _ListsNode<T> &);
	};

	template <typename T>
	struct _ListsNode {
		friend class Lists<T>;
		~_ListsNode() {
			if (nodeType == NodeType::LIST) { child.~shared_ptr(); }
		}
		_ListsNode(const T &value) : nodeType(NodeType::ATOM), atom(value), next(nullptr) { }
		_ListsNode() : nodeType(NodeType::LIST), next(nullptr) {
			new (&child) std::shared_ptr<_ListsNode<T>>(nullptr);
		}
		_ListsNode(const _ListsNode<T> &src) : nodeType(src.nodeType), next(src.next) {
			if (nodeType == NodeType::ATOM)
				atom = src.atom;
			else child = src.child;
		}
	private:
		enum class NodeType { ATOM, LIST };
		NodeType nodeType;
		union {
			T atom;
			std::shared_ptr<_ListsNode<T>> child;
		};
		std::shared_ptr<_ListsNode<T>> next;
	};

	template <typename T>
	Lists<T>::Lists(const std::string &expression)
	{
		std::istringstream in(expression);
		if (in.get() == '(')
			create_lists(in, this->root);
		else throw std::invalid_argument("illegal format when parsing the string");
	}

	template <typename T>
	Lists<T>::Lists(Lists &&src) noexcept
	{
		root = src->root;
		src->root = nullptr;
	}

	template <typename T>
	std::string Lists<T>::to_string() const
	{
		std::ostringstream out;
		out << '(';
		if (root->child != nullptr)
			to_string(out, *(root->child));
		out << ')';
		return out.str();
	}

	template <typename T>
	void Lists<T>::to_string(std::ostream &out, const _ListsNode<T> &node)
	{
		if (node.nodeType == _ListsNode<T>::NodeType::ATOM) {
			out << node.atom;
		}
		else {
			if (node.child != nullptr) {
				out << '(';
				to_string(out, *(node.child));
			}
			out << ')';
		}
		if (node.next != nullptr) {
			out << ',';
			to_string(out, *(node.child));
		}
	}

	template <typename T>
	void Lists<T>::for_each(const std::function<void(const T&)> &func) const
	{
		if (root->child != nullptr)
			traverse(*(root->next));
	}

	template <typename T>
	void Lists<T>::for_each(const std::function<void(T&)> &func)
	{
		if (root->child != nullptr)
			traverse(*(root->next));
	}

	template <typename T>
	void Lists<T>::traverse(_ListsNode<T> &node, const std::function<void(T&)> &func)
	{
		if (node.nodeType == _ListsNode<T>::NodeType::ATOM) {
			func(node.atom);
		}
		else {
			if (node.child != nullptr)
				traverse(*(node.child));
		}
		if (node.next != nullptr)
			traverse(*(node.next));
	}

	template <typename T>
	Lists<T> Lists<T>::parse(const std::string &expression)
	{
		return Lists<T>(expression);
	}

	template <typename T>
	void Lists<T>::create_lists(std::istream &in, std::shared_ptr<_ListsNode<T>> &current)
	{
		auto exception = std::invalid_argument("illegal format when parsing the string");
		char buf;
		in >> buf;
		if (buf == '(') {
			current = std::make_shared<_ListsNode<T>>();
			create_lists(in, current->child);
		}
		else if (buf == ')') {
			current->child = current->next = nullptr;
		}
		else if (buf != ',') {
			in.unget();
			T atomValue;
			in >> atomValue;
			current = std::make_shared<_ListsNode<T>>(atomValue);
		}
		else throw exception;
		if (!(in >> buf))
			throw exception;
		if (buf == ')') {
			current->next = nullptr;
		}
		else if (buf == ',') {
			create_lists(in, current->next);
		}
		else throw exception;
	}
}