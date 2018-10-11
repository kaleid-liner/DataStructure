#pragma once
#include <memory>
#include <functional>
#include <utility>

namespace DataStructures {
	template <typename T> // where T : comparable
	class AVLTree {
	private:
		template <typename U>
		struct TreeNode {
			U value;
			size_t factor;
			std::unique_ptr<TreeNode<U>> left;
			std::unique_ptr<TreeNode<U>> right;
			TreeNode(const TreeNode<U> &rhs)
				: value(rhs.value), factor(rhs.factor)
			{
				if (rhs.left != nullptr)
					left.reset(new TreeNode<U>(*rhs.left));
				if (rhs.right != nullptr)
					right.reset(new TreeNode<U>(*rhs.right));
			}
			TreeNode<U> &operator=(const TreeNode<U> &rhs)
			{
				value = rhs.value;
				factor = rhs.factor;
				left.reset(new TreeNode<U>(*rhs.left));
				right.reset(new TreeNode<U>(*rhs.right));
				return *this;
			}
			TreeNode(TreeNode<U> &&) = default;
			explicit TreeNode(const U &val) : value(val) { }
			explicit TreeNode(U &&val) : value(std::move(val)) { }
			~TreeNode() = default;
			bool contains(const U &val) const
			{
				if (val == value)
					return true;
				if (val < value && left != nullptr)
					return left->contains(val);
				if (right != nullptr)
					return right->contains(val);
			}
		};

		static void traverse(const std::function<void(const T&)> &, const std::unique_ptr<TreeNode<T>> &) const;

		template <typename U>
		static bool doInsert(std::unique_ptr<TreeNode<T>> &, U &&, bool &);

		static void leftBalance(std::unique_ptr<TreeNode<T>> &);
		static void rightBalance(std::unique_ptr<TreeNode<T>> &);
		static void leftRotation(std::unique_ptr<TreeNode<T>> &);
		static void rightRotation(std::unique_ptr<TreeNode<T>> &);

		std::unique_ptr<TreeNode<T>> root;


	public:
		using value_type = T;
		AVLTree() = default;
		AVLTree(const AVLTree<T> &);
		AVLTree(AVLTree<T> &&) noexcept;
		~AVLTree() = default;

		bool insert(const T &value);
		bool insert(T &&value);
		bool contains(const T &value) const;
		void for_each(const std::function<void(const T&)> &) const;
	};

	template <typename T>
	AVLTree<T>::AVLTree(const AVLTree<T> &rhs)
	{
		if (rhs != nullptr)
			root.reset(new TreeNode<T>(rhs.root));
	}

	template <typename T>
	AVLTree<T>::AVLTree(AVLTree<T> &&rhs) noexcept
		: root(std::move(rhs.root)) { }

	template <typename T>
	bool AVLTree<T>::insert(T &&value)
	{
		bool taller;
		return doInsert(root, std::move(value), taller);
	}

	template <typename T>
	bool AVLTree<T>::insert(const T&value)
	{
		bool taller;
		return doInsert(root, value, taller);
	}

	template <typename T>
	void AVLTree<T>::for_each(const std::function<void(const T&)> &func) const
	{
		traverse(func, root->left);
		traverse(func, root->right);
	}

	template <typename T>
	bool AVLTree<T>::contains(const T&value) const
	{
		if (root == nullptr)
			return false;
		else return root->contains(value);
	}

	template <typename T>
	void AVLTree<T>::traverse(const std::function<void(const T&)> &func, const std::unique_ptr<TreeNode<T>> &node) const
	{
		if (node != nullptr)
		{
			traverse(func, node->left);
			traverse(func, node->right);
		}
	}

	template <typename T>
	template <typename U>
	bool AVLTree<T>::doInsert(std::unique_ptr<TreeNode<T>> &node, U &&val, bool &taller)
	{
		if (node == nullptr)
		{
			node.reset(new TreeNode<T>(std::forward(val)));
			return true;
		}
		if (val == value) return false;
		if (val < value)
		{
			if (!doInsert(node->left, val, taller)) return false;
			if (taller)
			{
				switch (node->factor)
				{
				case 1:
					leftBalance(node);
					taller = false;
					break;
				case 0:
					node->factor = 1;
					taller = true;
					break;
				case -1:
					node->factor = 0;
					taller = false;
					break;
				}
			}
		}
		else
		{
			if (!doInsert(node->right, val, taller)) return false;
			if (taller)
			{
				switch (node->factor)
				{
				case -1:
					rightBalance(node);
					taller = false;
					break;
				case 0:
					node->factor = -1;
					taller = true;
					break;
				case -1:
					node->factor = 0;
					taller = false;
					break;
				}
			}
		}
		return true;
	}

	template <typename T>
	void AVLTree<T>::leftBalance(std::unique_ptr<TreeNode<T>> &node)
	{
		const auto &lc = node->left;
		switch (lc->factor)
		{
		case 1:
			node->factor = lc->factor = 0;
			rightRotation(node);
			break;
		case -1:
			const auto &lrc = lc->right;
			switch (lrc->factor)
			{
			case -1:
				lc->factor = 1;
				lrc->factor = 0;
				node->factor = 0;
				break;
			case 1:
				node->factor = -1;
				lc->factor = 0;
				lrc->factor = 0;
				break;
			}
			leftRotation(lc);
			rightRotation(node);
		}
	}

	template <typename T>
	void AVLTree<T>::rightBalance(std::unique_ptr<TreeNode<T>> &node)
	{
		const auto &rc = node->right;
		switch (rc->factor)
		{
		case -1:
			node->factor = rc->factor = 0;
			leftRotation(node);
			break;
		case 1:
			const auto &rlc = rc->left;
			switch (rlc->factor)
			{
			case 1:
				rc->factor = -1;
				rlc->factor = 0;
				node->factor = 0;
				break;
			case -1:
				node->factor = -1;
				rc->factor = 0;
				rlc->factor = 0;
				break;
			}
			rightRotation(rc);
			leftRotation(node);
		}
	}

	template <typename T>
	void AVLTree<T>::leftRotation(std::unique_ptr<TreeNode<T>> &node)
	{
		auto &new_root = node->right;
		node->right.reset(new_root->left);
		new_root->left.reset(node);
		node.reset(new_root);
	}

	template <typename T>
	void AVLTree<T>::rightRotation(std::unique_ptr<TreeNode<T>> &node)
	{
		auto new_root = std::move(node->left);
		node->left.reset(new_root->right);
		new_root->right.reset(node);
		node.reset(new_root);
	}
}
