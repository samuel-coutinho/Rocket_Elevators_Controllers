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
        @destinationList = @destinationList.shift
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

col1 = Column.new(10, 2)
col1.createElevatorList
col1.createcallButtonList
#e1 = Elevator.new(5)
requestedFloor = 3
requestedDirection = 'Up'
elevatorDirection = 'Up'
elevator = 0
col1.requestElevator(requestedFloor, requestedDirection)