using System;
using System.Collections.Generic;

[Serializable]
abstract public class Record
{
    public List<RecordUnit> scores;
    public double average;

    public Record() {
        scores = new List<RecordUnit>();
    }
    
    /// <summary>
    /// when it is called, this method add Score and maintain its time.
    /// </summary>
    /// <param name="sc">score</param>
    public abstract void AddScore(int sc);

    protected void SetAverage()
    {
        int sum = 0;
        foreach (RecordUnit score in scores)
        {
            sum += score.score;
        }

        average = sum / scores.Count;
    }
}

[Serializable]
public class TrainingRecord : Record
{
    public TRAINING_TYPE type;
    public int difficulty;

    public TrainingRecord(TRAINING_TYPE type)
    {
        this.type = type;
        difficulty = 0;
    }

    public override void AddScore(int sc)
    {
        scores.Add(new RecordUnit(sc,difficulty));
       //SetDifficulty(sc);
        SetAverage();
    }

    /// <summary>
    /// This method refer to xml file.
    /// </summary>
    /// <param name="sc">score that client recieved.</param>
    private void SetDifficulty(int sc)
    {
        //임시
        if (sc <= 2 && difficulty > 1)
            difficulty--;
        else if (sc >= 4 && difficulty < 5)
            difficulty++;
    }
}

[Serializable]
public class EvaluationRecord : Record
{
    public EVALUATION_TYPE type;

    public EvaluationRecord(EVALUATION_TYPE type)
    { 
        this.type = type;
    }
    public override void AddScore(int sc)
    {
        scores.Add(new RecordUnit(sc));
        SetAverage();
    }
}

[Serializable]
public class RecordUnit
{
    /// <summary>
    /// When this class is generated, 'date' is saved with that time.
    /// </summary>
    public Date date;

    public int score;

    public int difficulty;

    public RecordUnit(int sc)
    {
        score = sc;
        date = new Date();
    }

    public RecordUnit(int sc, int difficulty)
    {
        score = sc;
        date = new Date();
        this.difficulty = difficulty;
    }

    public override string ToString()
    {
        return date.dayString;
    }
}

