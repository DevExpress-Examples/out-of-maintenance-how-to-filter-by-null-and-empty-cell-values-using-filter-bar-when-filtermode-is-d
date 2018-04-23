using DevExpress.Data.Filtering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class CriteraVisitorBase : IClientCriteriaVisitor<CriteriaOperator> {
    public CriteraVisitorBase() { }

    #region IClientCriteriaVisitor Members

    public virtual CriteriaOperator Visit(JoinOperand theOperand) {
        CriteriaOperator condition = theOperand.Condition.Accept(this) as CriteriaOperator;
        CriteriaOperator expression = theOperand.AggregatedExpression.Accept(this) as CriteriaOperator;
        if (object.ReferenceEquals(condition, null) || object.ReferenceEquals(expression, null)) return null;
        return new JoinOperand(theOperand.JoinTypeName, condition, theOperand.AggregateType, expression);
    }

    public virtual CriteriaOperator Visit(OperandProperty theOperand) {
        return theOperand;
    }

    public virtual CriteriaOperator Visit(AggregateOperand theOperand) {
        OperandProperty operand = theOperand.CollectionProperty.Accept(this) as OperandProperty;
        CriteriaOperator condition = theOperand.Condition.Accept(this) as CriteriaOperator;
        CriteriaOperator expression = theOperand.AggregatedExpression.Accept(this) as CriteriaOperator;
        if (object.ReferenceEquals(condition, null) || object.ReferenceEquals(expression, null) || object.ReferenceEquals(operand, null)) return null;
        return new AggregateOperand(operand, expression, theOperand.AggregateType, condition);
    }
    #endregion

    #region ICriteriaVisitor Members

    public virtual CriteriaOperator Visit(FunctionOperator theOperator) {
        List<CriteriaOperator> operators = new List<CriteriaOperator>();
        foreach (CriteriaOperator op in theOperator.Operands) {
            CriteriaOperator temp = op.Accept<CriteriaOperator>(this);
            if (object.ReferenceEquals(temp, null)) return null;
            operators.Add(temp);
        }
        return new FunctionOperator(theOperator.OperatorType, operators);
    }

    public virtual CriteriaOperator Visit(OperandValue theOperand) {
        return theOperand;
    }

    public virtual CriteriaOperator Visit(GroupOperator theOperator) {
        List<CriteriaOperator> operators = new List<CriteriaOperator>();
        foreach (CriteriaOperator op in theOperator.Operands) {
            CriteriaOperator temp = op.Accept<CriteriaOperator>(this);
            if (object.ReferenceEquals(temp, null)) continue;
            operators.Add(temp);
        }
        return new GroupOperator(theOperator.OperatorType, operators);
    }

    public virtual CriteriaOperator Visit(InOperator theOperator) {
        CriteriaOperator leftOperand = theOperator.LeftOperand.Accept<CriteriaOperator>(this);
        List<CriteriaOperator> operators = new List<CriteriaOperator>();
        foreach (CriteriaOperator op in theOperator.Operands) {
            CriteriaOperator temp = op.Accept<CriteriaOperator>(this);
            if (object.ReferenceEquals(temp, null)) continue;
            operators.Add(temp);
        }
        if (object.ReferenceEquals(leftOperand, null)) return null;
        return new InOperator(leftOperand, operators);
    }

    public virtual CriteriaOperator Visit(UnaryOperator theOperator) {
        CriteriaOperator operand = theOperator.Operand.Accept<CriteriaOperator>(this);
        if (object.ReferenceEquals(operand, null)) return null;
        return new UnaryOperator(theOperator.OperatorType, operand);
    }

    public virtual CriteriaOperator Visit(BinaryOperator theOperator) {
        CriteriaOperator leftOperand = theOperator.LeftOperand.Accept<CriteriaOperator>(this);
        CriteriaOperator rightOperand = theOperator.RightOperand.Accept<CriteriaOperator>(this);
        if (object.ReferenceEquals(leftOperand, null) || object.ReferenceEquals(rightOperand, null)) return null;
        return new BinaryOperator(leftOperand, rightOperand, theOperator.OperatorType);
    }

    public virtual CriteriaOperator Visit(BetweenOperator theOperator) {
        CriteriaOperator test = theOperator.TestExpression.Accept<CriteriaOperator>(this);
        CriteriaOperator begin = theOperator.BeginExpression.Accept<CriteriaOperator>(this);
        CriteriaOperator end = theOperator.EndExpression.Accept<CriteriaOperator>(this);
        if (object.ReferenceEquals(test, null) || object.ReferenceEquals(begin, null) || object.ReferenceEquals(end, null)) return null;
        return new BetweenOperator(test, begin, end);
    }
    #endregion
}