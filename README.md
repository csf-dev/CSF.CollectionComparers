# Collection comparers
This library offers three implementations of `IEqualityComparer<T>`:

* `ListEqualityComparer<T>`
* `BagEqualityComparer<T>`
* `SetEqualityComparer<T>`

These represent three different strategies by which `IEnumerable<T>` objects may be compared for equality.

## List equality
Two enumerable objects are equal using the list equality strategy if:

* All items in both compared objects are equal
    * All items in A must be present in B, and vice-versa
* Any duplicated items are present in the same quantity in both objects
* The order of the items is the same across both compared objects

## Bag equality
Two enumerable objects are equal using the bag equality strategy if:

* All items in both compared objects are equal
    * All items in A must be present in B, and vice-versa
* Any duplicated items are present in the same quantity in both objects
* *The order of the items is irrelevant*

## Set equality
Two enumerable objects are equal using the set equality strategy if:

* All items in both compared objects are equal
    * All items in A must be present in B, and vice-versa
* *Duplicated items are irrelevant*
    * An item may appear in one object many times but in the other only once and the collections would still be considered equal
* *The order of the items is irrelevant*