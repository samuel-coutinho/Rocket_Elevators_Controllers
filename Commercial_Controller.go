package main

import (
	"fmt"
	"sort"
)

type Battery struct {
	id                       int
	status                   string
	numberColumns            int
	numberElevatorsPerColumn int
	numberBasements          int
	numberFloors             int
	columnList               []Column
}

func (b *Battery) Init(_id int, _numberColumns int, _numberElevatorsPerColumn int, _numberBasements int, _numberFloors int) {
	b.id = _id
	b.status = "Online"
	b.numberColumns = _numberColumns
	b.numberElevatorsPerColumn = _numberElevatorsPerColumn
	b.numberBasements = _numberBasements
	b.numberFloors = _numberFloors
	b.CreateColumnList()
}

//Creates the columnList, the first column is generated manually to contain all the basements, the others are generated with a FOR loop
func (b *Battery) CreateColumnList() {
	newColumn := new(Column)
	newColumn.Init(0, b.numberBasements, b.numberElevatorsPerColumn, -b.numberBasements, -1)
	b.columnList = append(b.columnList, *newColumn)

	numberOfFloorsPerColumn := b.numberFloors / (b.numberColumns - 1)
	for i := 1; i < b.numberColumns; i++ {
		firstServedFloor := 0
		lastServedFloor := 0
		if i == 1 { // For the first column, the formula is diferent to exclude the Lobby
			firstServedFloor = 2
			lastServedFloor = firstServedFloor + (numberOfFloorsPerColumn - 2)
		} else {
			firstServedFloor = 1 + (i-1)*numberOfFloorsPerColumn               //Ex(i = 2): 1 + (2 -1) * 20 = 21
			lastServedFloor = firstServedFloor + (numberOfFloorsPerColumn - 1) //EX(i = 2): 21 + (20 -1) = 40
		}
		//If there are floors left, they are all placed in the last column
		reminderFloors := b.numberFloors % (b.numberColumns - 1)
		if i == b.numberColumns-1 && reminderFloors != 0 {
			lastServedFloor += reminderFloors
		}
		//Creates the column object end add it to the columnList
		newColumn := new(Column)
		newColumn.Init(i, numberOfFloorsPerColumn, b.numberElevatorsPerColumn, firstServedFloor, lastServedFloor)
		b.columnList = append(b.columnList, *newColumn)
	}
}

type Column struct {
	id                int
	status            string
	numberFloors      int
	numberOfElevators int
	firstServedFloor  int
	lastServedFloor   int
	elevatorList      []Elevator
}

func (c *Column) Init(_id int, _numberFloors int, _numberOfElevators int, _firstServedFloor int, _lastServedFloor int) {
	c.id = _id
	c.status = "Online"
	c.numberFloors = _numberFloors
	c.numberOfElevators = _numberOfElevators
	c.firstServedFloor = _firstServedFloor
	c.lastServedFloor = _lastServedFloor
	c.numberFloors = _numberFloors

	//Creates the elevatorList
	for i := 0; i < c.numberOfElevators; i++ {
		newElevator := new(Elevator)
		newElevator.Init(i)
		c.elevatorList = append(c.elevatorList, *newElevator)
	}
}
func (c *Column) findBestElevator(requestedFloor int) int {
	shortestDistance := c.numberFloors
	var bestElevatorId int
	distance := 0
	bestElevatorFound := false

	for i := 0; i < c.numberOfElevators; i++ {
		if c.elevatorList[i].elevatorDirection == "Down" && requestedFloor <= c.elevatorList[i].currentFloor && c.id > 0 {
			if requestedFloor == c.elevatorList[i].currentFloor && c.elevatorList[i].doors == "Open" {
				bestElevatorId = i
				return bestElevatorId
			}
			distance = goAbs(requestedFloor - c.elevatorList[i].currentFloor)
			if distance <= shortestDistance && distance != 0 {
				shortestDistance = distance
				bestElevatorId = i
				bestElevatorFound = true
			}
		}
	}
	if bestElevatorFound {
		return bestElevatorId
	}
	for j := 0; j < c.numberOfElevators; j++ {
		if requestedFloor == c.elevatorList[j].currentFloor && c.elevatorList[j].doors == "Open" {
			bestElevatorId = j
			return bestElevatorId
		}
		if c.elevatorList[j].elevatorDirection == "Up" && requestedFloor >= c.elevatorList[j].currentFloor && c.id < 1 {
			distance = goAbs(c.elevatorList[j].currentFloor - requestedFloor)
			if distance <= shortestDistance && distance != 0 {
				shortestDistance = distance
				bestElevatorId = j
				bestElevatorFound = true
			}
		}
	}
	if bestElevatorFound {
		return bestElevatorId
	}
	for k := 0; k < c.numberOfElevators; k++ {
		if requestedFloor == c.elevatorList[k].currentFloor && c.elevatorList[k].doors == "Open" {
			bestElevatorId = k
			return bestElevatorId
		}
		distance = goAbs(requestedFloor - c.elevatorList[k].currentFloor)
		if distance <= shortestDistance {
			shortestDistance = distance
			bestElevatorId = k
		}
	}
	return bestElevatorId
}

type Elevator struct {
	id                int
	status            string
	elevatorDirection string
	currentFloor      int
	doors             string
	destinationList   []int
}

func (e *Elevator) Init(_id int) {
	e.id = _id
	e.status = "Online"
	e.elevatorDirection = "None"
	e.currentFloor = 1
	e.doors = "Closed"
}

// Set the next destination and move the elevator
func (e *Elevator) goToDestinationFloor(_requestedFloor int, _elevatorDirection string, columnId int) {

	e.destinationList = append(e.destinationList, _requestedFloor)
	e.elevatorDirection = _elevatorDirection

	// If direction is Up, sorts the list from least to largest.
	if e.elevatorDirection == "Up" {
		sort.Ints(e.destinationList)
		//destinationList.Sort();
	}
	// If direction is Down, sorts the list from largest to least.
	if e.elevatorDirection == "Down" {
		sort.Sort(sort.Reverse(sort.IntSlice(e.destinationList)))
		//destinationList.Reverse();
	}
	// Set the next destination to the first element of sorted list.
	destination := e.destinationList[0]
	columnLetter := "None"
	switch columnId {
	case 0:
		columnLetter = "A"
	case 1:
		columnLetter = "B"
	case 2:
		columnLetter = "C"
	case 3:
		columnLetter = "D"
	}
	fmt.Println("***************************************")
	fmt.Printf("Column = %s\n", columnLetter)
	fmt.Printf("Elevator = %s%d\n", columnLetter, (e.id + 1))

	if e.currentFloor == 1 {
		fmt.Printf("Current floor = Ground Floor\n")
	} else {
		fmt.Printf("Current floor = %d\n", e.currentFloor)
	}
	fmt.Printf("Direction = %s \n", e.elevatorDirection)
	e.doors = "Closed"
	fmt.Printf("Closing Doors\n\n")

	//while currentFloor != destination	{
	//for e.currentFloor = true; e.currentFloor; e.currentFloor = destination {
	for e.currentFloor != destination {
		if e.elevatorDirection == "Up" {
			e.currentFloor++
		}
		if e.elevatorDirection == "Down" {
			e.currentFloor--
		}
	}
	// Removes the reached floor from destinationList and puts the direction in None (Idle)
	e.destinationList = e.destinationList[1:]
	e.elevatorDirection = "None"
	fmt.Printf("Column = %s\n", columnLetter)
	fmt.Printf("Elevator = %s%d\n", columnLetter, (e.id + 1))
	if e.currentFloor == 1 {
		fmt.Printf("Current floor = Ground Floor\n")
	} else {
		fmt.Printf("Current floor = %d\n", e.currentFloor)
	}
	e.doors = "Open"
	fmt.Printf("Opening Doors\n\n")
	fmt.Println("***************************************")

}

func goAbs(x int) int {
	if x < 0 {
		return -x
	}
	return x
}

func main() {

	BatteryOne := new(Battery)
	BatteryOne.Init(1, 4, 5, 6, 60)
	BatteryOne.CreateColumnList()

	//Test
	x := BatteryOne.columnList[1].findBestElevator(15)
	fmt.Println(x)
}
