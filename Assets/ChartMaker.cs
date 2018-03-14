using System.Collections.Generic;
using UnityEngine;

public class ChartMaker : MonoBehaviour {

    public BarChart barchart;
    private List<TrainingRecord> training;

	// Use this for initialization
	void Start () {

        List<TrainingRecord> training = new List<TrainingRecord>();
        TrainingRecord training1 = new TrainingRecord(TRAINING_TYPE.COIN_BANK);
        training1.AddScore(2);
        training1.AddScore(3);
        training1.AddScore(4);
        training1.AddScore(2);
        training1.AddScore(5);
        TrainingRecord training2 = new TrainingRecord(TRAINING_TYPE.DRESS_CHOICE);
        training2.AddScore(1);
        training2.AddScore(2);
        training2.AddScore(3);
        training2.AddScore(4);
        training2.AddScore(5);
        TrainingRecord training3 = new TrainingRecord(TRAINING_TYPE.PICTURE_PUZZLE);
        training3.AddScore(5);
        training3.AddScore(2);
        training3.AddScore(3);
        training3.AddScore(2);
        training3.AddScore(4);

        training.Add(training1);
        training.Add(training2);
        training.Add(training3);

        this.training = training;

        //you have to change in the ChangeChartScale's method.
        barchart.MakeChart(training,
            ExtractRecordListUnitFromTrainingRecord,
            ExtractIntFromRecordUnit,
            ExtractNameFromTrainingRecord
            );
    }

    public List<RecordUnit> ExtractRecordListUnitFromTrainingRecord(TrainingRecord tr)
    {
        return tr.scores;
    }

    public List<string> ExtractNameFromTrainingRecord(List<TrainingRecord> list)
    {
        List<string> names = new List<string>();

        for (int i = 0; i < list.Count; i++)
            names.Add(list[i].type.ToString("G"));

        return names;
    }

    public int ExtractIntFromRecordUnit(RecordUnit unit)
    {
        return unit.score;
    }

    //you should make this form. Depend on its MakeChartType.
    public void Refresh()
    {
        //use UpdateLines.
        barchart.UpdateLines(training,
            ExtractRecordListUnitFromTrainingRecord,
            ExtractIntFromRecordUnit,
            ExtractNameFromTrainingRecord
            );
    }
    
}
