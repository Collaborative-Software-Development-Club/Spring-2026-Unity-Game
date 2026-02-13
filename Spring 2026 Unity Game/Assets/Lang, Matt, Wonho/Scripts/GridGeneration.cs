using UnityEngine;

public class GridGeneration : MonoBehaviour
{
    public char[,] grid;
    private string letters = " ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    public string exampleMessage = "I REALLY HOPE THIS WORKS";
    public int[,] angles = {{1,1},{1,-1},{-1,1},{-1,-1},{1,0},{0,1},{-1,0},{0,-1}};
    public int startX;
    public int startY;
    public int endX;
    public int endY;
    public int currentX;
    public int currentY;
    bool [,] visited;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startX = Random.Range(0,13);
        currentX = startX;
        startY = Random.Range(0,13);
        currentY = startY;
        grid = new char[13,13];
        visited = new bool[13,13];
        for (int i = 0; i < 13; i++) {
            for(int j = 0; j<13; j++) {
                grid[i,j] = '0';
            }
        }
        int retryCount = 0;
        int maxRetries = 100;

        bool pathCreated = populatePath(exampleMessage);
        while(!pathCreated && retryCount < maxRetries) {
            clearGarbage();
            pathCreated = populatePath(exampleMessage);
            retryCount++;
            Debug.Log("Retry attempt: " + retryCount);
        }

        if (!pathCreated) {
            Debug.Log("Failed to generate path after " + maxRetries + " attempts");
        }
        for (int i = 0; i< 13; i++) {
            for(int j = 0; j<13; j++) {
                if (!visited[i,j]) {
                    grid[i,j] = randomLetter();
                }
            }
        }
        string []grids = new string[13];
        for (int i = 0; i < 13; i++)
        {
            for (int j = 0; j < 13; j++)
            {
                grids[i] += grid[i, j] + " ";
            }
            Debug.Log(grids[i]);
        }
    }
    void clearGarbage() {
        startX = Random.Range(0,13);
        currentX = startX;
        startY = Random.Range(0,13);
        currentY = startY;
        grid = new char[13,13];
        visited = new bool[13,13];
    }
    char randomLetter()
    {
        int letter = Random.Range(0, 26);
        return letters[letter];
    }
    bool inBounds(int x, int xChange, int y, int yChange) {
        return (0 <= x + xChange && 12 >= x + xChange) && (0 <= y + yChange && 12 >= y + yChange);
    }
    int[] pickValidAngle(int x, int y)
    {
        int max = angles.GetLength(0);
        bool valid = false;
        int [] angle = {0,0};
        int maxAttempts = 50;
        int attempt = 0;
        while (attempt < maxAttempts && !valid) {
            int chosen = Random.Range(0, max);
            angle[0] = angles[chosen,0];
            angle[1] = angles[chosen,1];
            if (inBounds(x, angle[0], y, angle[1]) && avoidOverlaps(angle[0],angle[1])) {
                valid = true;
            }
            attempt += 1;
        }
        if (attempt >= maxAttempts) {
            return new int[] {0,0};
        }
        return angle;
    }
    bool populatePath(string message)
    {
        int currentPosition = 0;
        int x = currentX;
        int y = currentY;
        int maxTotalAttempts = 1000;
        int totalAttempts = 0;
        while (currentPosition < message.Length)
        {
            totalAttempts++;
            if (totalAttempts > maxTotalAttempts)
            {
                Debug.Log("Too Many Attempts");
                return false; 
            }
            int[] angle = pickValidAngle(x, y);
            //no valid angle found - restart
            if (angle[0] == 0 && angle[1] == 0)
            {
                return false;
            }
            x += angle[0];
            y += angle[1];
            if (totalAttempts == 1) {
                startX = x;
                startY = y;
            }
            while (currentPosition < message.Length && inBounds(x, 0, y, 0) && !visited[x, y])
            {
                visited[x, y] = true;
                grid[x, y] = message[currentPosition];
                currentPosition++;

                x += angle[0];
                y += angle[1];
            }
            x -= angle[0];
            y -= angle[1];
        }
        endX = x;
        endY = y;
        string []grids = new string[13];
        for (int i = 0; i < 13; i++)
        {
            for (int j = 0; j < 13; j++)
            {
                grids[i] += grid[i, j] + " ";
            }
            Debug.Log(grids[i]);
        }
        currentX = x;
        currentY = y;
       
        return true;
    }
    bool avoidOverlaps(int dx, int dy) {
        int x = currentX + dx;
        int y = currentY + dy;
        while(inBounds(x,0,y,0)) {
            if (visited[x,y]) {
                return false;
            }
            x+= dx;
            y += dy;
        }
        return true;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
