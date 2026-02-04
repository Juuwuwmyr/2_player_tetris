# 2-Player Console Tetris Game

A console-based Tetris game implemented in C# with two-player competitive gameplay where players battle by clearing lines to reduce their opponent's HP.

## Game Features

- **2-Player Mode**: Two players compete on the same console screen
- **HP System**: Each player starts with 10 HP, loses HP when opponent clears lines
- **Progressive Difficulty**: Game speeds up as players clear more lines
- **Distinct Controls**: 
  - Player 1: WASD for movement, Q for rotation
  - Player 2: Arrow keys for movement, N for rotation
- **Visual Design**: All-white blocks using `[]` symbols for better visibility

## **MGA DATA STRUCTURES AT ALGORITHMS NA GINAMIT (TAGLISH)**

### **MGA DATA STRUCTURES:**

#### 1. **2D Arrays (Two-Dimensional Arrays)**
- **Sa Game**: Ginamit para sa game board (12x28 grid)
- **Sa Code**: `int[,] cells` sa Board class
- **Purpose**: Mag-store ng estado ng bawat cell (empty or occupied)
- **Benefits**: Fast access O(1), memory efficient, easy collision detection

#### 2. **Queue (Pila - FIFO: First In, First Out)**
- **Sa Game**: Next piece preview system
- **Sa Code**: `Queue<TetrominoType> nextPieces` sa Player class
- **Operations**: 
  - `Enqueue()` - Mag-add ng bagong piece sa dulo
  - `Dequeue()` - Kunin ang susunod na piece sa simula
  - `ToArray()` - Preview upcoming pieces
- **Benefits**: Fair piece distribution, predictable gameplay

#### 3. **Stack (Stack - LIFO: Last In, First Out)**
- **Sa Game**: Game history at event logging
- **Sa Code**: `Stack<string> gameHistory` sa Game class
- **Operations**:
  - `Push()` - Mag-add ng event (line clears, moves)
  - `Pop()` - Remove latest event (potential undo)
  - `Peek()` - View latest event without removing
- **Benefits**: Event tracking, potential undo system

#### 4. **List with Sorting**
- **Sa Game**: High score ranking system
- **Sa Code**: `List<HighScore> highScores` sa Game class
- **Implementation**: `IComparable` interface para sa automatic sorting
- **Benefits**: Automatic descending order by score, timestamp tracking

#### 5. **Classes/Objects (Object-Oriented Programming)**
- **Sa Game**: Encapsulation ng game components
- **Classes**: Game, Player, Board, Tetromino, HighScore
- **Benefits**: Modularity, reusability, clear separation of concerns

#### 6. **3D Arrays**
- **Sa Game**: Tetromino shapes storage
- **Sa Code**: `int[,,] shapes` sa Tetromino class
- **Purpose**: Mag-store ng lahat ng 7 tetromino types with rotations
- **Benefits**: Efficient shape management, easy rotation

### **MGA ALGORITHMS:**

#### 1. **Matrix Rotation Algorithm**
- **Sa Game**: Piece rotation (90-degree clockwise)
- **Sa Code**: `RotateTetromino()` method sa Tetromino class
- **Formula**: `temp[j, SIZE-1-i] = shape[i, j]`
- **Time Complexity**: O(n²) where n=4 (piece size)
- **Application**: When player presses rotation key (Q/N)

#### 2. **Collision Detection Algorithm**
- **Sa Game**: Movement validation at boundary checking
- **Sa Code**: `CheckCollision()` method sa Player class
- **Process**: 
  - Check board boundaries
  - Check occupied cells
  - Return true if collision, false if valid
- **Time Complexity**: O(1) for 4x4 pieces
- **Application**: Every move (left/right/down/rotate)

#### 3. **Line Clearing Algorithm**
- **Sa Game**: Remove completed horizontal lines
- **Sa Code**: `ClearFullLines()` method sa Board class
- **Process**:
  - Check each row if completely filled
  - Move all lines above down (gravity effect)
  - Return count of lines cleared
- **Time Complexity**: O(h × w) where h=height, w=width
- **Application**: Automatic after piece placement

#### 4. **Sorting Algorithm (TimSort Hybrid)**
- **Sa Game**: High score ranking
- **Sa Code**: `highScores.Sort()` using built-in .NET sorting
- **Algorithm**: Hybrid of Merge Sort and Insertion Sort
- **Time Complexity**: O(n log n) average case
- **Features**: Stable sorting, efficient for partially sorted data
- **Application**: Game over - display ranked scores

#### 5. **Queue Operations (FIFO Implementation)**
- **Sa Game**: Piece sequence management
- **Process**: 
  - Dequeue current piece
  - Enqueue new random piece
  - Maintain 5-piece preview
- **Time Complexity**: O(1) for enqueue/dequeue
- **Application**: Continuous piece flow during gameplay

#### 6. **Stack Operations (LIFO Implementation)**
- **Sa Game**: Event history tracking
- **Process**: 
  - Push events with timestamps
  - Potential pop for undo functionality
  - Peek for latest event review
- **Time Complexity**: O(1) for push/pop operations
- **Application**: Game analytics and replay potential

#### 7. **Speed Progression Algorithm**
- **Sa Game**: Increasing difficulty over time
- **Formula**: `newSpeed = INITIAL_SPEED - (lines/LINES_PER_LEVEL) × SPEED_DECREMENT`
- **Bounds**: `Math.Max(newSpeed, MIN_SPEED)`
- **Application**: Piece falling speed increases as players clear more lines

#### 8. **Input Handling Algorithm**
- **Sa Game**: Dual-player simultaneous input processing
- **Process**: 
  - Non-blocking key detection
  - Process all available input in one frame
  - Map keys to player actions
- **Features**: Debouncing, distinct player controls
- **Application**: Real-time gameplay responsiveness

## Data Structures Used

### 1. **2D Arrays for Game Board**
```csharp
private int[,] cells;
```
- **Purpose**: Represents the game board grid (12x28)
- **Implementation**: `int[BOARD_HEIGHT, BOARD_WIDTH]` array where each cell stores:
  - `0` = Empty space
  - `1-7` = Different tetromino types
- **Benefits**: 
  - Fast random access O(1)
  - Memory efficient for grid-based games
  - Easy collision detection and line clearing

### 2. **Structs/Classes for Game Objects**

#### Tetromino Class
```csharp
public class Tetromino
{
    private static readonly int[,,] shapes;  // 3D array for all piece shapes
    public int[,] GetRotatedShape();         // Returns current rotation state
}
```
- **Purpose**: Represents Tetris pieces with their shapes and rotations
- **Data Structure**: 3D array `int[7,4,4]` storing all 7 tetromino types
- **Algorithms**: Matrix rotation for piece rotation

#### Player Class
```csharp
public class Player
{
    private Board board;           // Player's game board
    private Tetromino currentPiece; // Currently falling piece
    private Queue<TetrominoType> nextPieces; // Queue for next pieces
}
```
- **Purpose**: Encapsulates all player-specific data and behavior
- **Composition**: Contains Board and Tetromino objects
- **State Management**: Tracks HP, score, and game state

#### Board Class
```csharp
public class Board
{
    private int[,] cells;          // 2D grid representation
    public int ClearFullLines();   // Line clearing algorithm
}
```
- **Purpose**: Manages the game board state and operations
- **Encapsulation**: Hides internal grid representation

### 3. **Game State Management**
```csharp
public class Game
{
    private Player player1;
    private Player player2;
    private int currentGameSpeed;
    private int totalLinesCleared;
    private Stack<string> gameHistory;
    private List<HighScore> highScores;
}
```
- **Purpose**: Central game controller managing overall state
- **State Tracking**: Speed progression, line counts, game over conditions

## Key Algorithms Implemented

### 1. **Matrix Rotation Algorithm**
```csharp
// Rotate 90 degrees clockwise
for (int i = 0; i < SIZE; i++) {
    for (int j = 0; j < SIZE; j++) {
        temp[j, SIZE - 1 - i] = shape[i, j];
    }
}
```
- **Purpose**: Rotate tetromino pieces
- **Time Complexity**: O(n²) where n is piece size (4x4)
- **Space Complexity**: O(n²) for temporary rotation matrix
- **Application**: Used when player presses rotation key

### 2. **Collision Detection Algorithm**
```csharp
private bool CheckCollision(int x, int y)
{
    for (int i = 0; i < Tetromino.SIZE; i++) {
        for (int j = 0; j < Tetromino.SIZE; j++) {
            if (shape[i, j] != 0) {
                int boardX = x + j;
                int boardY = y + i;
                // Check bounds and occupied cells
                if (boardX < 0 || boardX >= BOARD_WIDTH || 
                    boardY >= BOARD_HEIGHT ||
                    (boardY >= 0 && board.GetCell(boardX, boardY) != 0)) {
                    return true;
                }
            }
        }
    }
    return false;
}
```
- **Purpose**: Determine if piece can move to new position
- **Time Complexity**: O(1) - constant time for 4x4 pieces
- **Technique**: Boundary checking + cell occupancy verification
- **Usage**: Movement validation, piece placement detection

### 3. **Line Clearing Algorithm**
```csharp
public int ClearFullLines()
{
    int linesCleared = 0;
    for (int i = BOARD_HEIGHT - 1; i >= 0; i--) {
        bool isFull = true;
        // Check if line is completely filled
        for (int j = 0; j < BOARD_WIDTH; j++) {
            if (cells[i, j] == 0) {
                isFull = false;
                break;
            }
        }
        if (isFull) {
            MoveLinesDown(i, 1);
            linesCleared++;
            i++; // Recheck same line position
        }
    }
    return linesCleared;
}
```
- **Purpose**: Remove completed horizontal lines
- **Time Complexity**: O(h × w) where h=height, w=width
- **Gravity Effect**: `MoveLinesDown()` shifts all above lines down
- **Optimization**: Process from bottom up to avoid redundant checks

### 4. **Speed Progression Algorithm**
```csharp
// Update game speed based on lines cleared
int newSpeed = INITIAL_GAME_SPEED - (totalLinesCleared / LINES_PER_LEVEL) * SPEED_DECREMENT;
currentGameSpeed = Math.Max(newSpeed, MIN_GAME_SPEED);
```
- **Purpose**: Gradually increase difficulty
- **Formula**: Linear progression with configurable parameters
- **Bounds**: Ensures game doesn't become impossibly fast
- **Player Impact**: Affects piece falling speed

### 5. **Input Handling Algorithm**
```csharp
private void HandleInput()
{
    while (Console.KeyAvailable) {
        ConsoleKeyInfo key = Console.ReadKey(true);
        // Process each key press for respective player actions
    }
}
```
- **Purpose**: Handle simultaneous input from both players
- **Non-blocking**: Uses `Console.KeyAvailable` to prevent freezing
- **Key Mapping**: Distinct controls for each player
- **Debouncing**: Processes all available input in one frame

## Game Architecture Patterns

### 1. **Object-Oriented Design**
- **Encapsulation**: Each class manages its own state
- **Composition**: Player contains Board and Tetromino objects
- **Inheritance**: Potential for piece type specialization

### 2. **State Management**
- **Game States**: Menu, Playing, Paused, Game Over
- **Player States**: Active, Game Over
- **Piece States**: Falling, Placed, Cleared

### 3. **Separation of Concerns**
- **Game.cs**: Main game loop and state management
- **Player.cs**: Player-specific logic and state
- **Board.cs**: Board operations and rendering
- **Tetromino.cs**: Piece definitions and rotation
- **Input.cs**: User input handling (conceptual)

## Performance Considerations

### 1. **Memory Efficiency**
- 2D arrays for board representation
- Pre-computed tetromino shapes
- Minimal object creation during gameplay

### 2. **Time Complexity**
- **Movement**: O(1) collision checks
- **Rotation**: O(n²) matrix operations (n=4)
- **Line Clearing**: O(h × w) board traversal
- **Rendering**: O(h × w) console output

### 3. **Optimizations**
- Single frame input processing
- Efficient collision detection
- Progressive speed adjustment
- Minimal screen redraws

## Console Rendering Techniques

### 1. **Position-based Rendering**
```csharp
Console.SetCursorPosition(x, y);
Console.Write("[]");
```
- **Precision**: Exact cursor positioning for block placement
- **Efficiency**: Only render changed portions
- **Synchronization**: Coordinated rendering of both players

### 2. **Color Management**
- Consistent white color scheme for visibility
- Distinct border characters for board definition
- Clear visual hierarchy for game information

## Advanced Data Structures Implemented

### 1. **Queue for Next Piece Management**
```csharp
private Queue<TetrominoType> nextPieces;
```
- **Purpose**: FIFO (First In, First Out) management of upcoming tetromino pieces
- **Implementation**: `Queue<TetrominoType>` pre-filled with 5 pieces
- **Operations**: 
  - `Enqueue()`: Add new random piece to back of queue
  - `Dequeue()`: Remove and return next piece from front
  - `ToArray()`: Preview upcoming pieces
- **Benefits**: Predictable piece sequence, fair gameplay, easy extension

### 2. **Stack for Game History/Undo**
```csharp
private Stack<string> gameHistory;
```
- **Purpose**: LIFO (Last In, First Out) storage of game events
- **Implementation**: `Stack<string>` storing event descriptions with timestamps
- **Operations**:
  - `Push()`: Add game event (line clears, special moves)
  - `Pop()`: Remove most recent event (potential undo functionality)
  - `Peek()`: View latest event without removing
- **Application**: Game analytics, potential undo feature, event logging

### 3. **List with Sorting for High Scores**
```csharp
private List<HighScore> highScores;
```
- **Purpose**: Maintain and display ranked player scores
- **Implementation**: `List<HighScore>` with `IComparable` interface
- **Sorting Algorithm**: Built-in `Sort()` method using hybrid algorithm (typically TimSort)
- **Time Complexity**: O(n log n) average case
- **Features**: Automatic descending order by score, timestamp tracking

## Enhanced Features

### 1. **Dynamic Queue-Based Piece Preview**
- Shows next 3 pieces from queue
- Vertical arrangement for better visibility
- Real-time queue updates

### 2. **Game Event Logging**
- Automatic recording of line clears with timestamps
- Stack-based event history
- Potential for replay functionality

### 3. **Smart High Score System**
- Automatic sorting of all player scores
- Display of top 5 scores with rankings
- Date/time stamping for each score
- Descending order by default

## Educational Value

This implementation demonstrates:
- **Fundamental Data Structures**: Arrays, Queue, Stack, List, Classes
- **Classic Algorithms**: Matrix operations, collision detection, sorting
- **Game Development Patterns**: State machines, input handling
- **Performance Optimization**: Efficient algorithms and data access
- **Software Engineering**: Modular design, separation of concerns

The code serves as an excellent example of applying computer science fundamentals to game development while maintaining readability and extensibility.