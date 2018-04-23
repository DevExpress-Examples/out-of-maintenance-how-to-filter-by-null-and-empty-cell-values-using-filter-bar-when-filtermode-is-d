using DevExpress.Data.Filtering;
using System.Collections.Generic;

public static class CriteriaHelper {
    static ReplaceCriteriaVisitor visitor;
    static ReplaceCriteriaVisitor Visitor {
        get {
            if (visitor == null)
                visitor = new ReplaceCriteriaVisitor();
            return visitor;
        }
    }

    public static CriteriaOperator ReplaceCriteriaByFieldName(List<string> fieldsToReplace, CriteriaOperator op) {
        return Visitor.ReplaceCriteriaByFieldName(fieldsToReplace, op);
    }
}

public class ReplaceCriteriaVisitor : CriteraVisitorBase {
    List<string> fieldsToReplace;
    public ReplaceCriteriaVisitor() { }

    public CriteriaOperator ReplaceCriteriaByFieldName(List<string> fieldsToReplace, CriteriaOperator op) {
        if (fieldsToReplace.Count == 0) return null;
        this.fieldsToReplace = fieldsToReplace;
        return op.Accept<CriteriaOperator>(this);
    }

    public override CriteriaOperator Visit(UnaryOperator theOperator) {
        CriteriaOperator operand = theOperator.Operand.Accept<CriteriaOperator>(this);
        if (object.ReferenceEquals(operand, null)) return null;

        if (operand is OperandProperty) {
            string propertyName = (operand as OperandProperty).PropertyName;
            if (fieldsToReplace.Contains(propertyName) && theOperator.OperatorType == UnaryOperatorType.IsNull)
                return new FunctionOperator(FunctionOperatorType.IsNullOrEmpty, operand);
        }
        return base.Visit(theOperator);
    }
}
