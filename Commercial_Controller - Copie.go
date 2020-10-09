package main

import "fmt"

type Battery struct {
	id                       int
	status                   string
	numberColumns            int
	numberElevatorsPerColumn int
	numberBasements          int
	numberFloors             int
	columnList               []int
}

func (b *Battery) Init(_id int, _numberColumns int, _numberElevatorsPerColumn int, _numberBasements int, _numberFloors int) {
	b.id = _id
	b.status = "Online"
	b.numberColumns = _numberColumns
	b.numberElevatorsPerColumn = _numberElevatorsPerColumn
	b.numberBasements = _numberBasements
	b.numberFloors = _numberFloors
}

type Column struct {
	id                int
	status            string
	numberFloors      int
	numberOfElevators int
	firstServedFloor  int
	lastServedFloor   int
	columnList        []int
}

func (c *Column) Init(_id int, _numberFloors int, _numberOfElevators int, _firstServedFloor int, _lastServedFloor int) {
	c.id = _id
	c.status = "Online"
	c.numberFloors = _numberFloors
	c.numberOfElevators = _numberOfElevators
	c.firstServedFloor = _firstServedFloor
	c.lastServedFloor = _lastServedFloor
	c.numberFloors = _numberFloors
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

func main() {
	BatteryOne := new(Battery)
	BatteryOne.Init(1, 4, 5, 6, 60)
	ColumnTest := new(Column)
	ColumnTest.Init(0, 20, 5, 2, 20)
	ElevatorTest := new(Elevator)
	ElevatorTest.Init(1)

	fmt.Println(ElevatorTest)
}
