#Main menu
def main()
    option1 = 0      
    while option1 != 4
        puts"""Select one of the following test scenarios option:
        1) Scenario 1
        2) Scenario 2
        3) Scenario 3
        4) Exit)\n"""
        option1 = gets.to_i
        #Prevents numbers out of the range(1-3)
        while option1 < 1 || option1 > 4
            puts 'Please provide a number between 1 and 4'
            option1 = gets
        end
        #Option 1 of menu
        if option1 == 1
            Scenario1()
        end           
        #Option 2 of menu
        if option1 == 2
            Scenario2()
        end
        if option1 == 3
            Scenario3()
        end
    end
end

# Classes definition
class Column
    attr_accessor :status, :numberFloors, :numberOfElevators, :elevatorList, :callButtonList     
    def initialize(numberFloors, numberOfElevators)
        @status = 'Online'
        @numberFloors = numberFloors
        @numberOfElevators = numberOfElevators
        @elevatorList = Array.new
        @callButtonList = Array.new        
    end
    def createElevatorList()
        for i in 0..@numberOfElevators - 1            
            @elevatorList.push(Elevator.new(i))
        end
    end    
    def createcallButtonList()
        for i in 0..@numberFloors + 1                                     
            @callButtonList.push(CallButton.new(i))
        end
    end
    def requestElevator(requestedFloor, requestedDirection)          
        bestElevator = findBestElevator(requestedFloor, requestedDirection)      
        requestFloor(bestElevator.id, requestedFloor)
    end
    def requestFloor(elevator, requestedFloor)       
        puts 'Closing doors'                                           
        if requestedFloor > @elevatorList[elevator].currentFloor
            elevatorDirection = 'Up'
        end
        if requestedFloor < @elevatorList[elevator].currentFloor
            elevatorDirection = 'Down'
        end       
        @elevatorList[elevator].goToDestinationFloor(requestedFloor, elevatorDirection)
    end
    def findBestElevator(requestedFloor, requestedDirection)
        shortestDistance = @numberFloors
        bestElevatorId = 0

        for i in 0..@numberOfElevators - 1
            if requestedDirection == 'Up' && @elevatorList[i].elevatorDirection == 'Up' && requestedFloor > @elevatorList[i].currentFloor
                distance = abs(requestedFloor - @elevatorList[i].currentFloor)    
                if distance <= shortestDistance
                    shortestDistance = distance
                    bestElevatorId = i
                end
            end
        end
        if  bestElevatorId > 0
            return @elevatorList[bestElevatorId]
        end

        for i in 0..@numberOfElevators - 1
            if requestedDirection == 'Down' && @elevatorList[i].elevatorDirection == 'Down' && requestedFloor < @elevatorList[i].currentFloor
                distance = abs(requestedFloor - @elevatorList[i].currentFloor)    
                if distance <= shortestDistance
                    shortestDistance = distance
                    bestElevatorId = i
                end
            end
        end                    
        if  bestElevatorId > 0
            return @elevatorList[bestElevatorId]
        end

        for i in 0..@numberOfElevators - 1
            if @elevatorList[i].elevatorDirection == 'None'
                distance = (requestedFloor - @elevatorList[i].currentFloor).abs 
                if distance <= shortestDistance
                    shortestDistance = distance
                    bestElevatorId = i
                end
            end                    
            return @elevatorList[bestElevatorId]
        end
    end       
end

class Elevator
    attr_accessor :id, :status, :elevatorDirection, :currentFloor, :doors, :destinationList 
    def initialize(id)
        @id = id
        @status = 'Online'         
        @elevatorDirection = 'None' 
        @currentFloor = 1
        @doors = 'Closed'   
        @destinationList = Array.new 
    end    
    
    def goToDestinationFloor(requestedFloor, elevatorDirection)
        @destinationList.push(requestedFloor)
        @elevatorDirection = elevatorDirection 
        if @elevatorDirection == 'Up'       
            @destinationList.sort()
        end
        if @elevatorDirection == 'Down'       
            @destinationList.reverse 
        end         
        destination = @destinationList[0]
        print 'Elevator = ', @id, "\n"
        print 'Current floor = ', @currentFloor, "\n"
        print 'Going ', @elevatorDirection , "\n"                                     
        while @currentFloor != destination             
            if @elevatorDirection =='Up'                
                @currentFloor += 1
            end
            if @elevatorDirection =='Down'                
                @currentFloor -= 1  
            end  
        end                   
        @destinationList.shift
        @elevatorDirection = 'None'
        print 'Elevator = ', @id, "\n"       
        print 'Destination floor = ', @currentFloor, "\n"        
        print 'Opening Doors', "\n"
    end  
end
class CallButton
    attr_accessor :direction, :floor
    def initialize(floor)    
        @direction = 'None'
        @floor = floor 
    end
end

#Scenarios functions
def Scenario1()
    columnOne = Column.new(10, 2)
    columnOne.createElevatorList
    columnOne.createcallButtonList
    columnNumber = columnOne     
    #Elevator A
    columnNumber.elevatorList[0].currentFloor = 2
    columnNumber.elevatorList[0].elevatorDirection = 'None'
    #Elevator B
    columnNumber.elevatorList[1].currentFloor = 6
    columnNumber.elevatorList[1].elevatorDirection = 'None'   

    #Someone is on floor 3 and wants to go to the 7th floor.
    floorOfPersonRequestingElevator = 3
    requestedFloor = 7
    callButtonDirection = 'Up'
    puts'***************************************************'
    puts'Scenario 1'
    puts'Elevator A is Idle at floor 2'
    puts'Elevator B is Idle at floor 6'
    puts'Someone is on floor 3 and wants to go to the 7th floor.'
    puts "Elevator A = 0 is expected to be sent."
    runScenario(columnNumber, columnNumber.elevatorList[0].currentFloor, columnNumber.elevatorList[0].elevatorDirection, columnNumber.elevatorList[1].currentFloor, columnNumber.elevatorList[1].elevatorDirection, floorOfPersonRequestingElevator, requestedFloor, callButtonDirection)    
    puts'***************************************************'  
end
def Scenario2()
    columnTwo = Column.new(10, 2)
    columnNumber = columnTwo   
    columnNumber.createElevatorList
    columnNumber.createcallButtonList   
    #Elevator A
    columnNumber.elevatorList[0].currentFloor = 10
    columnNumber.elevatorList[0].elevatorDirection = 'None'
    #Elevator B
    columnNumber.elevatorList[1].currentFloor = 3
    columnNumber.elevatorList[1].elevatorDirection = 'None'  

    #Someone is on the 1st floor and requests the 6th floor.
    floorOfPersonRequestingElevator = 1
    requestedFloor = 6
    callButtonDirection = 'Up'
    puts '***************************************************'
    puts 'Scenario 2'
    puts'Elevator A is Idle at floor 10'
    puts'Elevator B is Idle at floor 3'
    puts'Someone is on the 1st floor and requests the 6th floor.'
    puts"Elevator B = 1 should be sent."
    runScenario(columnNumber, columnNumber.elevatorList[0].currentFloor, columnNumber.elevatorList[0].elevatorDirection, columnNumber.elevatorList[1].currentFloor, columnNumber.elevatorList[1].elevatorDirection, floorOfPersonRequestingElevator, requestedFloor, callButtonDirection)    

    # 2 minutes later, someone else is on the 3rd floor and requests the 5th floor. Elevator B should be sent
       
    puts'2 minutes later, someone else is on the 3rd floor and requests the 5th floor.'
    puts'Elevator B = 1 should be sent.'    
    floorOfPersonRequestingElevator = 3
    requestedFloor = 5
    callButtonDirection = 'Up'
    #(columnNumber, elevator A currentFloor, elevator A elevatorDirection, elevator B currentFloor, elevator B elevatorDirection, floorOfPersonRequestingElevator, requestedFloor) 
    runScenario(columnNumber, columnNumber.elevatorList[0].currentFloor, columnNumber.elevatorList[0].elevatorDirection, columnNumber.elevatorList[1].currentFloor, columnNumber.elevatorList[1].elevatorDirection, floorOfPersonRequestingElevator, requestedFloor, callButtonDirection)    

    #Finally, a third person is at floor 9 and wants to go down to the 2nd floor. Elevator A should be sent.
    puts'Finally, a third person is at floor 9 and wants to go down to the 2nd floor.'
    puts'Elevator A = 0 should be sent.'
    floorOfPersonRequestingElevator = 9
    requestedFloor = 2    
    runScenario(columnNumber, columnNumber.elevatorList[0].currentFloor, columnNumber.elevatorList[0].elevatorDirection, columnNumber.elevatorList[1].currentFloor, columnNumber.elevatorList[1].elevatorDirection, floorOfPersonRequestingElevator, requestedFloor, callButtonDirection)    
    puts'***************************************************' 
end
def Scenario3()
    columnThree = Column.new(10, 2)
    columnNumber = columnThree   
    columnNumber.createElevatorList
    columnNumber.createcallButtonList     
    #Elevator A
    columnNumber.elevatorList[0].currentFloor = 10
    columnNumber.elevatorList[0].elevatorDirection = 'None'
    #Elevator B
    columnNumber.elevatorList[1].currentFloor = 3
    columnNumber.elevatorList[1].elevatorDirection = 'Up'
    columnNumber.elevatorList[1].destinationList.push(6)    

    #Someone is on floor 3 and wants to go to the 7th floor.
    floorOfPersonRequestingElevator = 3
    requestedFloor = 2
    callButtonDirection = 'Up'
    puts'***************************************************'
    puts'Scenario 3'
    puts'Elevator A is Idle at floor 10'
    puts'Elevator B is Moving from floor 3 to floor 6'
    puts'Someone is on floor 3 and requests the 2nd floor.'
    puts "Elevator A = 0 should be sent."
    
    #(columnNumber, elevator A currentFloor, elevator A elevatorDirection, elevator B currentFloor, elevator B elevatorDirection, floorOfPersonRequestingElevator, requestedFloor)
    runScenario(columnNumber, columnNumber.elevatorList[0].currentFloor, columnNumber.elevatorList[0].elevatorDirection, columnNumber.elevatorList[1].currentFloor, columnNumber.elevatorList[1].elevatorDirection, floorOfPersonRequestingElevator, requestedFloor, callButtonDirection)

    #5 minutes later, someone else is on the 10th floor and wants to go to the 3rd. Elevator B should be sent.
    puts'5 minutes later, someone else is on the 10th floor and wants to go to the 3rd.'
    puts'Elevator B should be sent.'
    
    floorOfPersonRequestingElevator = 10
    requestedFloor = 3
    callButtonDirection = 'Down'
    columnNumber.elevatorList[1].currentFloor = 6
    columnNumber.elevatorList[1].elevatorDirection = 'None'
    columnNumber.elevatorList[1].destinationList = columnNumber.elevatorList[1].destinationList.shift   

    runScenario(columnNumber, columnNumber.elevatorList[0].currentFloor, columnNumber.elevatorList[0].elevatorDirection, columnNumber.elevatorList[1].currentFloor, columnNumber.elevatorList[1].elevatorDirection, floorOfPersonRequestingElevator, requestedFloor, callButtonDirection)
    puts'***************************************************'   
end
def runScenario(columnNumber, elevatorAinitialFloor, elevatorAinitialDirection, elevatorBinitialFloor, elevatorBinitialDirection, floor, requestedFloor, callButtonDirection )        
      
        #Elevator A
        columnNumber.elevatorList[0].currentFloor = elevatorAinitialFloor
        columnNumber.elevatorList[0].elevatorDirection = elevatorAinitialDirection
        #Elevator B
        columnNumber.elevatorList[1].currentFloor = elevatorBinitialFloor
        columnNumber.elevatorList[1].elevatorDirection = elevatorBinitialDirection        

        #Request Elevator        
        columnNumber.callButtonList[floor].direction = callButtonDirection    
        columnNumber.requestElevator(floor, columnNumber.callButtonList[floor].direction)  
        puts"Please wait a moment"     
    
        #Request floor        
        print 'Going to floor ',requestedFloor, "\n"
        elevator = columnNumber.findBestElevator(floor, columnNumber.callButtonList[floor].direction)    
        columnNumber.requestFloor(elevator.id, requestedFloor)
end 
#-------Test Section-------#
main()
#Scenario1()
#Scenario3()