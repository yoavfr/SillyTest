using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ScopeRuntime;
using Microsoft.ProtectionServices.Entities.Raw;

public class RuleProcessor : Processor
{
    public override Schema Produces(string[] requestedColumns, string[] args, Schema inputSchema)
    {
        return new Schema("Sha1:string,EvaluatorName:string,Prediction:string,Actual:string");
    }

    public override IEnumerable<Row> Process(RowSet input, Row outputRow, string[] args)
    {
        foreach (Row row in input.Rows)
        {
            string Sha1 = row[0].String;
            List<RuleEvaluationResult> ruleEvaluationResults = (List<RuleEvaluationResult>) row[1].Value;


            foreach (RuleEvaluationResult ruleEvaluationResult in ruleEvaluationResults)
            {
                string EvaluatorName = ruleEvaluationResult.RuleName;
                string Prediction = ruleEvaluationResult.Determination.ToString();
                string Actual = "Clean"; //Oracle.GetActual(spyNetReport)

                outputRow[0].Set(Sha1);
                outputRow[1].Set(EvaluatorName);
                outputRow[2].Set(Prediction);
                outputRow[3].Set(Actual);
                yield return outputRow;
            }
        }
    }

}