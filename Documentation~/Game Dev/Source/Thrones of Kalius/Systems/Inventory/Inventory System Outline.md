
# Overview
This system should store and manage a collection of items belonging to the parent object.
This system should be able to add new items, remove items, stack items, unstack items, and use items.
The parent of this system could be a player, an enemy, a storage container (chest, crate, etc.), a vendor, a bank, or a quest reward.

# Components of the Inventory System

#### Inventory
##### Overview
This component contains information about the owner of the inventory, as well as a collection of inventory slots to contain the items within the inventory. This component will be responsible for the management of items, Add, Remove, Clear.

##### Properties
###### Entity owner - 
The object that this inventory belongs to. May need to change data type that is shared across all objects that contain an inventory, such as an interface.

###### int Capacity -
The maximum number of slots this inventory can hold. To be used in the future for expandable inventories.
###### List<> inventorySlots -
The collection of slots that will contain the items within the inventory.

##### Methods

###### public Entity GetOwner() -
Will return the owner of the inventory.

###### public void AddItem(Item item) -
Adds item to the collection.
if item is stackable, iterate through the collection to see if a slot containing the same item.ID already exists. If it does, check to see if we are able to stack without exceeding the item.MaxStackSize, if yes, add the item.Amount to inventorySlot[i].Amount. If stacking will exceed the maximum stack size, stack to maximum size and add the remaining amount to the first available slot.
If item is not stackable, add the item to the first available slot.

###### public void RemoveItem(Item item) -
Iterate through the collection checking each InventorySlot to see if item.ID is qual to inventorySlot[i].item.ID. If it is equal, clear the InventorySlot.

###### private void Clear() - 
iterates through the collection and clears each InventorySlot

###### private InventorySlot FindFirstEmpty() - 
Iterates through the collection, and returns the first empty InventorySlot.

#### InventorySlot
###### Overview
This component contains a reference to the item being contained within it. This component will handle the updating of the item it contains.

##### Properties

###### IInventorySystem parentInventory - 
A reference to the inventory system that this slot belongs to.

###### int slotIndex - 
A reference to the index of the slot within the parent InventorySystem's collection.

###### Item item - 
A reference to the item that is contained within this InventorySlot.

###### bool IsEmpty - 
boolean value to denote if the slot contains an item or not.

##### Methods

###### public void SetItem(Item item) - 
Set the item property to the item that is passed in.

###### public void Clear()
Clears the item that is contained within this InventorySlot

#### Item
##### Overview
This component contains the data for the item that is to be stored within the inventory.

##### Properties

###### String ID - 
A string identifier for each object.

###### String itemName - 
The name of the item.

###### String itemDescription - 
A short description of the item.

###### Image itemIcon - 
the visual representation of the item within an inventory.

###### int maxStackSize - 
The maximum stack size of this object.
If the item can not be stacked, MaxStackSize will be 1

###### int itemAmount - 
The current amount of items within this stack.

##### Methods

###### public void Add(int amount) - 
Add amount to this.itemAmount

###### public void Remove(int amount) - 
Remove amount from this.itemAmount

#### UIInventoryPanel
##### Overview
This component is responsible for the visual representation of the inventory system. This component will also handle the manipulation of items within the inventory through mouse input.

##### Properties

###### IInventorySystem inventory - 
The inventory that is to be displayed

###### GameObject slotPrefab - 
Reference to the InventorySlot prefab.