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

func (b *Battery) requestElevator(requestedFloor int) {
	// Calls the functions that finds the bestColumn and bestElevator
	bestColumn := b.columnList[b.findBestColumn(requestedFloor)]
	bestElevatorId := bestColumn.findBestElevator(requestedFloor)

	// Calls the function to define the elevatorDirection
	elevatorDirection := b.defineElevatorDirection(requestedFloor, bestColumn.id, bestElevatorId)

	// Calls the function that routes the elevator to the person and then to the Lobby(1)
	bestColumn.elevatorList[bestElevatorId].goToDestinationFloor(requestedFloor, elevatorDirection, bestColumn.id)
	bestColumn.elevatorList[bestElevatorId].goToDestinationFloor(1, elevatorDirection, bestColumn.id)
}
func (b *Battery) assignElevator(requestedFloor int) {
	// Calls the functions that finds the bestColumn and bestElevator
	bestColumn := b.columnList[b.findBestColumn(requestedFloor)]
	bestElevatorId := bestColumn.findBestElevator(1)
	// Calls the function to define the elevatorDirection
	elevatorDirection := b.defineElevatorDirection(1, bestColumn.id, bestElevatorId)
	//Calls the function that routes the elevator to the Lobby(1)
	bestColumn.elevatorList[bestElevatorId].goToDestinationFloor(1, elevatorDirection, bestColumn.id)
	// Sends the elevator to requested floor
	elevatorDirection = b.defineElevatorDirection(requestedFloor, bestColumn.id, bestElevatorId)
	bestColumn.elevatorList[bestElevatorId].goToDestinationFloor(requestedFloor, elevatorDirection, bestColumn.id)
}
func (b *Battery) findBestColumn(_requestedFloor int) int {
	// Checks which column has the range to which the desired floor belongs
	var bestColumnId int
	for i := 0; i < b.numberColumns; i++ {
		if _requestedFloor >= b.columnList[i].firstServedFloor && _requestedFloor <= b.columnList[i].lastServedFloor {
			bestColumnId = i
		}
	}
	return bestColumnId
}

// Defines the elevatorDirection comparing the requestedFloor and the current elavatorFloor
func (b *Battery) defineElevatorDirection(requestedFloor, bestColumnId, bestElevatorId int) string {
	elevatorDirection := b.columnList[bestColumnId].elevatorList[bestElevatorId].elevatorDirection
	elavatorFloor := b.columnList[bestColumnId].elevatorList[bestElevatorId].currentFloor
	if requestedFloor > elavatorFloor {
		elevatorDirection = "Up"
	} else if requestedFloor < elavatorFloor {
		elevatorDirection = "Down"
	}
	return elevatorDirection
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

	// Looks for an elevator going down (condition 1) on a higher level than the floor requested (condition 2) when it is not in the basements (condition 3)
	for i := 0; i < c.numberOfElevators; i++ {
		if c.elevatorList[i].elevatorDirection == "Down" && requestedFloor <= c.elevatorList[i].currentFloor && c.id > 0 {
			// Checks if there is an elevator on the requested floor with the doors still open
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
	} // If the first search has a result, returns it
	if bestElevatorFound {
		return bestElevatorId
	}

	for j := 0; j < c.numberOfElevators; j++ {
		// Checks if there is an elevator on the requested floor with the doors still open
		if requestedFloor == c.elevatorList[j].currentFloor && c.elevatorList[j].doors == "Open" {
			bestElevatorId = j
			return bestElevatorId
		}
		// Looks for an elevator going up (condition 1) on a lower level than the floor requested (condition 2) when it is in the basements (condition 3)
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
	} // If previous searches have not been successful, looks for the nearest elevator within the column
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

// Function to return the absolute value of a integer
func goAbs(x int) int {
	if x < 0 {
		return -x
	}
	return x
}

// Scenarios Functions
func runScenarioOne() {
	BatteryOne := new(Battery)
	BatteryOne.Init(1, 4, 5, 6, 60)
	BatteryOne.CreateColumnList()
	fmt.Println("Scenario 1")

	//Scenario 1:
	//Elevator B1 at 20th floor going to the 5th floor
	BatteryOne.columnList[1].elevatorList[0].currentFloor = 20
	BatteryOne.columnList[1].elevatorList[0].elevatorDirection = "Down"
	// Elevator B2 at 3rd floor going to the 15th floor
	BatteryOne.columnList[1].elevatorList[1].currentFloor = 3
	BatteryOne.columnList[1].elevatorList[1].elevatorDirection = "Up"
	// Elevator B3 at 13th floor going to RC
	BatteryOne.columnList[1].elevatorList[2].currentFloor = 13
	BatteryOne.columnList[1].elevatorList[2].elevatorDirection = "Down"
	// Elevator B4 at 15th floor going to the 2nd floor
	BatteryOne.columnList[1].elevatorList[3].currentFloor = 15
	BatteryOne.columnList[1].elevatorList[3].elevatorDirection = "Down"
	// Elevator B5 at 6th floor going to RC
	BatteryOne.columnList[1].elevatorList[4].currentFloor = 6
	BatteryOne.columnList[1].elevatorList[4].elevatorDirection = "Down"

	// Someone at RC wants to go to the 20th floor.
	requestedFloor := 20
	BatteryOne.assignElevator(requestedFloor)
	// Elevator B5 is expected to be sent.
}
func runScenarioTwo() {
	BatteryTwo := new(Battery)
	BatteryTwo.Init(1, 4, 5, 6, 60)
	BatteryTwo.CreateColumnList()
	fmt.Println("Scenario 2")

	// Scenario 2:
	// Elevator C1 at RC going to the 21st floor (not yet departed)
	BatteryTwo.columnList[2].elevatorList[0].currentFloor = 1
	BatteryTwo.columnList[2].elevatorList[0].elevatorDirection = "Up"

	// Elevator C2 at 23rd floor going to the 28th floor
	BatteryTwo.columnList[2].elevatorList[1].currentFloor = 23
	BatteryTwo.columnList[2].elevatorList[1].elevatorDirection = "Up"
	// Elevator C3 at 33rd floor going to RC
	BatteryTwo.columnList[2].elevatorList[2].currentFloor = 33
	BatteryTwo.columnList[2].elevatorList[2].elevatorDirection = "Down"
	// Elevator C4 at 40th floor going to the 24th floor
	BatteryTwo.columnList[2].elevatorList[3].currentFloor = 40
	BatteryTwo.columnList[2].elevatorList[3].elevatorDirection = "Down"
	// Elevator C5 at 39th floor going to RC
	BatteryTwo.columnList[2].elevatorList[4].currentFloor = 39
	BatteryTwo.columnList[2].elevatorList[4].elevatorDirection = "Down"

	// Someone at RC wants to go to the 36th floor.
	requestedFloor := 36
	BatteryTwo.assignElevator(requestedFloor)
	// Elevator C1 is expected to be sent.
}
func runScenarioThree() {
	BatteryThree := new(Battery)
	BatteryThree.Init(1, 4, 5, 6, 60)
	BatteryThree.CreateColumnList()
	fmt.Println("Scenario 3")

	// Scenario 3:
	// Elevator D1 at 58th going to RC
	BatteryThree.columnList[3].elevatorList[0].currentFloor = 58
	BatteryThree.columnList[3].elevatorList[0].elevatorDirection = "Down"
	// Elevator D2 at 50th floor going to the 60th floor
	BatteryThree.columnList[3].elevatorList[1].currentFloor = 50
	BatteryThree.columnList[3].elevatorList[1].elevatorDirection = "Up"
	// Elevator D3 at 46th floor going to the 58th floor
	BatteryThree.columnList[3].elevatorList[2].currentFloor = 46
	BatteryThree.columnList[3].elevatorList[2].elevatorDirection = "Up"
	// Elevator D4 at RC going to the 54th floor
	BatteryThree.columnList[3].elevatorList[3].currentFloor = 1
	BatteryThree.columnList[3].elevatorList[3].elevatorDirection = "Up"
	// Elevator D5 at 60th floor going to RC
	BatteryThree.columnList[3].elevatorList[4].currentFloor = 60
	BatteryThree.columnList[3].elevatorList[4].elevatorDirection = "Down"

	// Someone at 54e floor wants to go to RC.
	requestedFloor := 54
	BatteryThree.requestElevator(requestedFloor)
	// Elevator D1 is expected to be sent.
}
func runScenarioFour() {
	BatteryFour := new(Battery)
	BatteryFour.Init(1, 4, 5, 6, 60)
	BatteryFour.CreateColumnList()
	fmt.Println("Scenario 4")

	// Scenario 4:
	// Elevator A1 “Idle” at SS4
	BatteryFour.columnList[0].elevatorList[0].currentFloor = -4
	BatteryFour.columnList[0].elevatorList[0].elevatorDirection = "None"
	// Elevator A2 “Idle” at RC
	BatteryFour.columnList[0].elevatorList[1].currentFloor = 1
	BatteryFour.columnList[0].elevatorList[1].elevatorDirection = "None"
	// Elevator A3 at SS3 going to SS5
	BatteryFour.columnList[0].elevatorList[2].currentFloor = -3
	BatteryFour.columnList[0].elevatorList[2].elevatorDirection = "Down"
	// Elevator A4 at SS6 going to RC
	BatteryFour.columnList[0].elevatorList[3].currentFloor = -6
	BatteryFour.columnList[0].elevatorList[3].elevatorDirection = "Up"
	// Elevator A5 at SS1 going to SS6
	BatteryFour.columnList[0].elevatorList[4].currentFloor = -1
	BatteryFour.columnList[0].elevatorList[4].elevatorDirection = "Down"

	// Someone at SS3 wants to go to RC.
	requestedFloor := -3
	BatteryFour.requestElevator(requestedFloor)
	// Elevator A4 is expected to be sent.
}
func main() {

	//runScenarioOne()
	//runScenarioTwo()
	//runScenarioThree()
	runScenarioFour()
}
