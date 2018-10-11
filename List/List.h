#ifndef _MY_LIST_H

#include <iostream>
#include <iterator>
#include <exception>

namespace DataStructures {
	template <typename T> struct _ListNode;

	template <typename T>
	class List {
	public:
		using size_t = size_t;
		using value_type = T;
		using iterator = _List_iterator<List<T>>;
		using const_iterator = _List_const_iterator<List<T>>;

		size_t size() const {
			size_t cnt = 0;
			for (auto beg = cbegin(); beg != cend(); ++beg)
				cnt++;
			return cnt;
		}
		bool empty() const {
			return root == nullptr;
		}
		const_iterator cbegin() const {
			return root;
		}
		const_iterator cend() const {
			return nullptr;
		}
		iterator begin() {
			return root;
		}
		iterator end() {
			return nullptr;
		}
		iterator insert(const T &value) { return doInsert(value); }
		iterator insert(T &&value) { return doInsert(value); }
		iterator insert(const_iterator where, const T &value) { return doInsertafter(where, value); }
		iterator insert(const_iterator where, T &&value) { return doInsertafter(where, value); }
		~List_iterator();

	private:
		_ListNode<T> *root;
		template <typename X>
		iterator doInsert(X &&val);

		template <typename X>
		iterator doInsertafter(const_iterator where, X &&val);
	};

	template <typename T>
	struct _ListNode {
		using value_type = T;

		T *_next;
		T _data;
		_ListNode() = default;
		_ListNode(const T &data) :
			_next(nullptr), _data(data) { }
		_ListNode(T &&data) :
			_next(nullptr), _data(std::move(data)) { }
	};

	template <typename MyList>
	class _List_const_iterator
	{
		friend bool operator==(const _List_const_iterator<T> &lhs, const _List_const_iterator<T> &rhs);
		friend bool operator!=(const _List_const_iterator<T> &lhs, const _List_const_iterator<T> &rhs);
		friend class MyList;

	public:
		using value_type = typename MyList::value_type;
		using pointer = value_type * ;
		using reference = value_type & ;
		using iterator_category = std::forward_iterator_tag;
		using difference_type = std::ptrdiff_t;

		_List_const_iterator<MyList> &operator++() {
			_ptr = _ptr->_next;
			return *this;
		}
		_List_const_iterator<MyList> &operator++(int) {
			auto temp = this;
			++(*this);
			return temp;
		}
		const value_type &operator*() const {
			if (_ptr == nullptr)
				throw std::out_of_range("List access out of range");
			return (*_ptr)->_data;
		}

	protected:
		_List_const_iterator() = default;
		_List_const_iterator(_ListNode<value_type> *ptr) : _ptr(ptr) { }
		_List_const_iterator(const _List_const_iterator<T> &iter) : _ptr(iter._ptr) { }

	private:
		_ListNode<typename MyList::value_type> * _ptr;
	};

	template <typename MyList>
	class _List_iterator : public _List_const_iterator<MyList> {
		friend class MyList;
	public:
		value_type & operator*() {
			return const_cast<value_type &>(
				*(static_cast<const _List_iterator<MyList> &>(*this))
				);
		}
	private:
		_List_iterator() : _List_const_iterator<MyList>() { };
		_List_iterator(_ListNode<value_type> *ptr) : 
			_List_const_iterator<MyList>(ptr) { }
		_List_iterator(const _ListNode<value_type> &iter):
			_List_const_iterator<MyList>(iter) { }
	};


	template <typename T>
	bool operator==(const _List_const_iterator<T> &lhs, const _List_const_iterator<T> &rhs)
	{
		return lhs._ptr == rhs._ptr;
	}

	template <typename T>
	bool operator!=(const _List_const_iterator<T> &lhs, const _List_const_iterator<T> &rhs)
	{
		return !(lhs == rhs);
	}

}

#endif
#define _MY_LIST_H
