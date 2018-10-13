#include <iostream>
#include "polynomial.h"

using namespace std;
using namespace DataStructure;


int main()
{
	freopen("D:\\code\\repo\\DataStructure\\Debug\\dataset.txt", "r", stdin);
	Polynomial a, b;
	cout << "Please input a and b..." << endl
		<< "Your input must follow this format : "
		<< "{an}x^{dn}+......+{a0}x^{d0}" << endl
		<< "where an must be a float point number, and dn must be an integer. And dn > dn-1 > d0" << endl;

	cout << "a : ";
	cin >> a;
	cout << "b : ";
	cin >> b;
	cout << "a is " << a << endl;
	cout << "b is " << b << endl;
	cout << "a + b is " << a + b << endl;
	cout << "a - b is " << a - b << endl;
	cout << "a * b is " << a * b << endl;
	cout << "a / b is " << a / b << endl;

	cout << "while x = 1, a = " << a.eval(1) << endl;
	cout << "the length of a is " << a.length() << endl;
	fclose(stdin);

}