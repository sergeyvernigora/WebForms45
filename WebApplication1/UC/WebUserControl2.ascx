<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebUserControl2.ascx.cs" Inherits="WebApplication1.UC.WebUserControl2" %>

<script type="text/javascript">

    //$('#ddlCollectionReason').change(getSubreasons);
    //$('#ddlModel').attr('disabled', true);
    //$('#ddlColour').attr('disabled', true);


    //function CheckedRadioButton() {
    //    $find("AuthorisationBehaviour").show();
    //}

    $(function () {
        $("[name$=ddlCollectionReason]").change(getSubreasons);
    });

    function getSubreasons() {
        var reason = $(this);
        var sendBox = $(this).closest('.reasonSandbox');
        var hidId = sendBox.find($("[name$=ID]"));
        var questionsDiv = sendBox.find(".Questions");

        $.ajax({
            type: "POST",
            url: "../CollectionReasonService.asmx/GetQuestions",
            data: "{orderLineId:" + hidId.val() + ", reasonId: " + reason.val() + " }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                var questions = (typeof response.d) == 'string' ? eval('(' + response.d + ')') : response.d;
                drawQuestion(questionsDiv, questions);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                console.log(xhr.status);
                console.log(thrownError);
            }
        });
    }

    function drawQuestion(qDiv, questions) {
        qDiv.empty();

        if (!questions)
            return;

        for (var i = 0; i < questions.length; i++) {

            var $row = $('.questionOption').filter(':hidden').clone();
            $(".qTitle", $row).html(questions[i].Text);

            if (questions[i].QuestionType == 2) {
                drawText($row, questions[i]);
            } else if (questions[i].QuestionType == 3) {
                drawCheck($row, questions[i]);
            } else {
                drawSelect($row, questions[i]);
            }

            $row.show();

            qDiv.append($row);
        }

        function drawSelect(row, question) {
            var selCase = $(".selectCase", row);
            var sel = $('select', selCase);

            $(question.Options).each(function () {
                var $item = $(this)[0];
                sel.append($("<option></option>")
                .attr("value", $item.Id)
                .attr("answerid", $item.Id)
                .attr("EnableFreeText", $item.EnableFreeText)
                .attr("mandatory", $item.Mandatory)
                .text($item.Text));
            });

            sel.attr('data-orderlineid', question.OrderLineId);
            sel.attr('data-reasonid', question.ReasonId);
            sel.attr('data-questionid', question.Id);
            sel.attr('data-questiontext', question.Text);
            sel.attr('data-questiontype', question.QuestionType);
            sel.prop('required', true);

            var id = 's_olid' + question.OrderLineId + 'rid' + question.ReasonId + 'qid' + question.Id;
            sel.attr('id', id);

            $(document).on('change', '#' + id, changeQuestion);
            $(document).on('change', '#' + id, saveReasonQuestions);

            selCase.show(changeQuestion);
        }



        function drawText(row, question) {
            var textCase = $(".textCase", row);
            var freeText = textCase.find(':input[type=text]').first();

            freeText.attr('data-orderlineid', question.OrderLineId);
            freeText.attr('data-reasonid', question.ReasonId);
            freeText.attr('data-questionid', question.Id);
            freeText.attr('data-questiontext', question.Text);
            freeText.attr('data-questiontype', question.QuestionType);
            freeText.prop('required', true);

            var id = 't_olid' + question.OrderLineId + 'rid' + question.ReasonId + 'qid' + question.Id;
            freeText.attr('id', id);

            $(document).on('change', '#' + id, saveReasonQuestions);

            textCase.show(changeQuestion);
        }

        function drawCheck(row, question) {
            var checkCase = $(".checkCase", row);
            var checkBox = checkCase.find(':input[type=checkbox]').first();

            checkBox.attr('data-orderlineid', question.OrderLineId);
            checkBox.attr('data-reasonid', question.ReasonId);
            checkBox.attr('data-questionid', question.Id);
            checkBox.attr('data-questiontext', question.Text);
            checkBox.attr('data-questiontype', question.QuestionType);


            var id = 'c_olid' + question.OrderLineId + 'rid' + question.ReasonId + 'qid' + question.Id;
            checkBox.attr('id', id);

            $(document).on('change', '#' + id, saveReasonQuestions);

            checkCase.show(changeQuestion);
        }

        function changeQuestion() {
            var select = $(this);
            var selectedItem = $("option:selected", select);
            select.attr('data-answerid', selectedItem.val());
            select.attr('data-selectfreetext', '');
            var freeText = select.siblings(':input[type=text]', select).first();

            freeText.val('');
            freeText.prop('required', false);
            freeText.hide();

            if (selectedItem.attr('EnableFreeText') === 'true') {
                select.attr('data-selectfreetext', true);

                freeText.show();

                freeText.attr('data-orderlineid', select.data('orderlineid'));
                freeText.attr('data-reasonid', select.data('reasonid'));
                freeText.attr('data-questionid', select.data('questionid'));
                freeText.attr('data-questiontext', select.data('questiontext'));
                freeText.attr('data-questiontype', select.data('questiontype'));
                freeText.attr('data-answerid', selectedItem.val());
                freeText.prop('required', true);

                var id = 'st_olid' + select.data('orderlineid') + 'rid' + select.data('reasonid') + 'qid' + select.data('questionid');
                freeText.attr('id', id);

                $(document).on('change', '#' + id, saveReasonQuestions);
            }
        }

        function saveReasonQuestions() {
            var ol = $(this).data('orderlineid');
            var rd = $(this).data('reasonid');

            var textCases = $(':input[type=text][data-orderlineid=' + ol + '][data-reasonid=' + rd + ']');
            var checkCases = $(':input[type=checkbox][data-orderlineid=' + ol + '][data-reasonid=' + rd + ']');
            var selectCases = $('select[data-orderlineid=' + ol + '][data-reasonid=' + rd + ']');

            var answers = [];

            var textAnswers = processAnswers(textCases);
            var checkAnswers = processAnswers(checkCases);
            var selectAnswers = processAnswers(selectCases);

            answers = $.merge(answers, textAnswers);
            answers = $.merge(answers, checkAnswers);
            answers = $.merge(answers, selectAnswers);

            var sendBox = $(this).closest('.reasonSandbox');
            var hidden = sendBox.find($("[name$=hidQuestionAnswers]"));

            hidden.val(JSON.stringify(answers));
        }

        function processAnswers(coll) {
            var answers = [];

            coll.each(function () {
                var $item = $(this);

                var val = '';
                var answerText = '';
                if ($item.is(':checkbox')) {
                    val = $item.prop("checked");
                } else if ($item.is('select') && $item.data('selectfreetext') == true) {
                    val = '';
                    answerText = $('#' + $item.attr('id') + ' option:selected').text();
                } else if ($item.is('select')) {
                    val = $item.val();
                    answerText = $('#' + $item.attr('id') + ' option:selected').text();
                }
                else {
                    val = $item.val();
                    answerText = $item.text();
                }

                answers.push({
                    "OrderLineId": $item.data('orderlineid'),
                    "ReasonId": $item.data('reasonid'),
                    "QuestionId": $item.data('questionid'),
                    "QuestionText": $item.data('questiontext'),
                    "QuestionType": $item.data('questiontype'),
                    "AnswerId": $item.data('answerid'),
                    "AnswerText": answerText,
                    "AnswerValue": val
                });
            });

            return answers;
        }
    }
</script>    

<div class="questionOption" style="display: none">
    <div class="">
        <strong class="qTitle"></strong>
    </div>
    <div class="checkCase" style="display: none">
        <input type="checkbox">
    </div>
    <div class="textCase" style="display: none">
        <strong class="textCaseTitle" style="display: none"></strong>
        <input type="text">
    </div>
    <div class="selectCase" style="display: none">
        <select class="required">
            <option value="" disabled="disabled">Choose an option</option>
        </select>
        <input type="text" style="display: none" >
    </div>
</div>

<script type="text/html" id="personTemplate">
       <div>
            <div style="float:left;"> ID : </div> <div>${UId} </div>
            <div style="float:left;"> Name : </div> <div>${Name} </div>
            <div style="float:left;"> Address : </div> <div>${Address} </div> <br />
       </div>
    
   </script>

<asp:Button ID="Button2" runat="server" Text="Button" />
 <asp:Button ID="Button1" runat="server" Text="Button" />
<asp:Repeater ID="rptProductsToBeCollected" runat="server" OnItemDataBound="ProductsToBeCollected_ItemDataBound">

    <ItemTemplate>
        <div class="reasonSandbox">
        <tr id="trOrderLine"  runat="server">
            <td id="Td1" runat="server">
                <asp:HiddenField ID="ID" runat="server" Value='<%# DataBinder.Eval(Container.DataItem, "Id") %>' />
            </td>
            <td id="Td2" runat="server">
                <%# DataBinder.Eval(Container.DataItem, "Description")%>
            </td>
            <td id="Td3" class='alignCenter' runat="server">

                <asp:DropDownList ID="ddlCollectionReason" runat="server"  />
                <asp:HiddenField ID="hidQuestionAnswers" runat="server" />
                <ul>
                <div class="Questions">
                </div>
                </ul>
            </td>
        </tr>
             </div>
    </ItemTemplate>
</asp:Repeater>

<input type="submit">
