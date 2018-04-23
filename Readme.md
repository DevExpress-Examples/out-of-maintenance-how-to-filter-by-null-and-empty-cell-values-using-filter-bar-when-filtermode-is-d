# How to filter by null and empty cell values using Filter Bar when FilterMode is DisplayText


<p>When a header filter is used for a column and its FilterMode is set to “DisplayText”, the predefined <em><strong>“(Blanks)”</strong></em> criterion returns rows with values equal to <strong>null</strong> or <strong>empty, </strong>whereas in a grid’s Filter Bar the <em><strong>“Is blank”</strong></em> condition will not return anything. </p>
<p>This can be described by the fact that in the former case, the resultant CriteriaOperator will be built as “<em>IsNullOrEmpty</em>”, while in the latter case it will be “<em>IsNull</em>”. In order to make Filter Bar work in the same way, it’s possible to parse its filter expression and replace <em>IsNull</em> operator with the required <em>IsNullOrEmpty</em>. </p>
<p>This example uses a parsing technique demonstrated in <a href="https://www.devexpress.com/Support/Center/p/E3396">E3396: How to delete all criteria corresponding to a particular field from CriteriaOperator</a>. In this case, the <strong><em>CriteriaHelper</em></strong> class was slightly modified so that the <em>UnaryOperator</em> of type <em>IsNull</em> is replaced by the <em>FunctionOperator</em> of type <em>IsNullOrEmpty</em>.</p>
<p>The modified filter expression is applied on a grid’s callback.</p>

<br/>


