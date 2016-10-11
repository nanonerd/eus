var TopicAnswers = React.createClass({
    getInitialState: function ()
    {
        var ia = this.props.initialAnswers;

        console.log('answers RatingCount: ' + ia[0].RatingCount);
        console.log('answers RatingScore: ' + ia[0].RatingScore);
        return {answers: this.props.initialAnswers};
    },
    loadAnswersFromServer: function () {
        var url = this.props.url;
        var answers = eusAPI.get(url)
        console.log('url: ' + url);
        console.log('answers: ' + answers);
        this.setState({answers: answers});


        //var result = eusAPI.post(url, jsonString);
        //
        //if (result.error == 'false') {
        //    alert('error is false');
        //    answerCount.text(result.count);
        //    answerScoreFinal.text(Math.round(result.score));
        //    $(".answer-commentRateBox").slideUp(300);
        //    thankYou.show(300);
        //    buttonAddCommentRate.text('Add comment/rate');
        //}
        //else {
        //    var error = {}; // Create object to hold error details
        //    error["user"] = "test";  // get userName
        //    error["detail"] = result.message;
        //    error["action"] = "trying to SetRating for topic for AnswerID: " + obj.AnswerID;
        //    error["errorLocation"] = "eusVote_TopicController > Index > script section (should later move to js file)";
        //    error["level"] = "high";
        //
        //    //alert('error on callback for SetRating');
        //
        //    // Create json string
        //    var errorData = JSON.stringify(error);
        //
        //    // call function to handle/log errors.
        //    eusAPI.errorHandle(errorData);
        //
        //    // send user a friendly error message !!!!!
        //}


    },
    //componentWillMount: function(){
    //    //this.loadAnswersFromServer();
    //    console.log('baseUrl: ' + this.props.baseUrl);
    //},
    componentDidMount: function () {
        // Star Rating
        var starCaptions = {
            0.5: 'hell (10)',
            1: 'blackhole (20)',
            1.5: 'crap (30)',
            2: 'mostly bad (40)',
            2.5: 'not happy (50)',
            3.0: 'average (60)',
            3.5: 'satisfied (70)',
            4: 'terrific (80)',
            4.5: 'love it (90)',
            5: 'heavenly (100)'
        }

        $(".rating").rating({starCaptions: starCaptions}, function () {
            var id = $(this).data('id');
        });

        // Show Rate/Comment button only if user is logged in. 0 means not logged in.
        var userID = this.props.userID;

        console.log('userID: ' + userID);
        console.log('initialAnswers: ' + this.state.answers);

        if (userID == 0) {
            console.log('userID is 0 - Not Logged In');
            $('.buttonRateComment').css("display", "none");
            $('.span-RateComment').show();
        }
        else {
            $('.buttonRateComment').show();
            $('.span-RateComment').hide();
        }
    },
    buttonSubmit: function (e) {
        // Grab userID from hidden field (will be '0' if not logged in)
        //var userID = document.getElementById('UserID').value;
        var userID = this.props.userID;

        var comment = e.comment;
        var answerID = e.answerID;
        var value = e.rating;

        // Translate rating value to score value (e.g., value = 4 > score = 80)
        var score = eusCommon.rating(value);

        // Create json array with UserID, AnswerID, and Rating
        var obj = {};
        obj["UserID"] = userID;
        obj["AnswerID"] = answerID; //8; //$(this).data('id');
        obj["Rating"] = score;
        obj["Comment"] = comment;

        // Create json string
        var data = JSON.stringify(obj);

        // Create json to send to API
        var jsonString = JSON.stringify({"Filter": "SetRating", "FilterData": data});
        var url = "/Vote/Topic/Topic";

        var result = eusAPI.post(url, jsonString);
        this.loadAnswersFromServer();
    },
    render: function () {
        return (
            <div>
                <Answers data={this.state.answers} baseUrl={this.props.baseUrl} topicID={this.props.topicID} topicTitle={this.props.topicTitle} submitHandlerButton={this.buttonSubmit} />
            </div>
        );
}
});

// Holds a list of all the answers
var Answers = React.createClass({
    render: function () {
        var self = this;
        var topicID = this.props.topicID;
        var topicTitle = this.props.topicTitle;
        var baseUrl = this.props.baseUrl;

        console.log('baseUrl: ' + baseUrl);

        var answerNodes = this.props.data.map(function (answer) {
            var submitHandlerFunction = function (answerData) {
                if (self.props.hasOwnProperty('submitHandlerButton')) {
                    self.props.submitHandlerButton({
                        answerID: answerData.answerID,
                        comment: answerData.comment,
                        rating: answerData.rating
                    });
                }
            };				
									
            return <Answer key={answer.AnswerID} id={answer.AnswerID} answerTitle={answer.Answer1} answerTitleURL={answer.AnswerTitleURL} detail={answer.Detail} baseUrl={baseUrl}
            score={answer.RatingScore} ratingCount={answer.RatingCount} commentCount={answer.CommentCount} topicID={topicID} topicTitle={topicTitle}
            handleSubmitAnswers={submitHandlerFunction} />
            });

        return (
            <div>
                {answerNodes}
            </div>
        );
    }
});

// The answer component that populates the Answers component
var Answer = React.createClass({
    getInitialState: function(){
        return { comment: "", rating: ""}
    },
    handleSubmitAnswer: function (e) {
        // NOTE: I want to use state for the rating akin to this.state.comment. However, I am not able to detect a change and thus,
        // I am unable to get onChange to trigger. Hence, I am using the "ref" method to get the value.
        //<input ref="rating" type="number" className="rating" value={this.state.rating} onChange={this.handleRatingChange}/>
        //var ratingValue = this.state.rating;   // NOTE: not able to use this because I cannot get the onChange to trigger in the rating Input element
        var ratingValue = this.refs.rating.value;
        var clickedButton = $(e.target);
        var rcBox = clickedButton.closest('.answer-container').find('.answer-rcBox');
        var submitMessage = clickedButton.closest('.answer-container').find('.answer-submitMessage');
        var spanButtonSubmitMsg = clickedButton.closest('.answer-rateIt').find('#span-buttonSubmitMsg');

        // Show message if not logged in
        if (ratingValue == 0){
            spanButtonSubmitMsg.html('&nbsp;&nbsp;&nbsp;Please rate, then submit').fadeIn(400);
        }
        else {
            // Submit Rating + Comment
            spanButtonSubmitMsg.hide();
            this.handlercBoxClick(e);

            if (this.props.hasOwnProperty('handleSubmitAnswers')) {
                this.props.handleSubmitAnswers({
                    answerID: this.props.id,
                    comment: this.state.comment,
                    rating: ratingValue
                });
            }

            // TODO - need check to see if all good before displaying submitMessage. How do I make this check?
            submitMessage.fadeIn(400);
        }
    },
    // Toggle the Rate/Comment button for each answer
    handlercBoxClick: function(e) {
        e.preventDefault(); // Only needed for forms

        // Get clicked element
        var targetElement = $(e.target);

        // Get the closest answer container
        var answerContainer = targetElement.closest('.answer-container');

        // Get the closest "Rate / Comment" button for the answer that was clicked
        var clickedButton = targetElement.closest('.answer-container').find('.buttonRateComment');

        // Get the closest "Rate Comment" box to the button that was clicked
        var rcBox = targetElement.closest('.answer-container').find('.answer-rcBox');

        // Get the original state of the button
        var buttonOriginalState = '';
        if (clickedButton.text() === 'Rate / Comment'){
            buttonOriginalState = 'Rate / Comment';
        }

        // Close and reset elements
        $('.buttonRateComment').html('Rate / Comment'); // Reset all button text to 'Rate / Comment'
        $('.answer-rcBox').hide(400);            // Close all Rate / Comment boxes
        $('.answer-submitMessage').hide(400);    // Close all Submit Messages
        $('.answer-container').css({'background-color': 'white'});

        // Set the text for the button
        if (clickedButton.text() === 'Rate / Comment'){
            $(answerContainer).css({'background-color': '#CEE3F6'})
            clickedButton.html('Cancel');
        }
        else{
            clickedButton.html('Rate / Comment');
        }

        // Based on the button original state, take action
        if (buttonOriginalState === 'Rate / Comment'){
            rcBox.show(400);
        }
        else{
            clickedButton.html('Rate / Comment');
        }
    },
    handleCommentChange: function(e){
        this.setState( {comment: e.target.value});
    },
    handleRatingChange: function(e){
        this.setState( { rating : e.target.value});
    },
    render: function () {
        var styleClear = {
            clear: 'both'
        };

        var displayNone = {
            display: 'none'
        };

        var msgLogin = {
            fontSize: 'small'
        };

        //var answerBackground = {
        //    //backgroundColor: '#E0F2F7',
        //    backgroundColor: '#CEE3F6',
        //    padding: 15
        //};

        var showCommentCount;

        if (this.props.commentCount == null){
            console.log("comment Count is null");
            //$('#answer-see-all-comments').hide();
            //document.getElementById("answer-see-all-comments").style.display='none';
            showCommentCount = displayNone;
        }
        else{
            console.log("comment count is: " + this.props.commentCount);
        }

        return (
            <div className="answer-container" >
                <div className="answer-main">
                    <h4>{this.props.answerTitle}</h4>
                        {this.props.detail}<br /><br />
            </div>

    <div className="answer-eusScore">
        <span>eusScore ({this.props.ratingCount})</span><br/><br/>
        <span className="span-answer-eusScore">{this.props.score}%</span>
    </div>

    <br style={styleClear}/>

    <button className="buttonRateComment" onClick={this.handlercBoxClick} >Rate / Comment</button><span className="span-RateComment" style={msgLogin}>[Login to rate/comment]</span><br/><br/>

    <div className="answer-rcBox" style={displayNone}>
        <div className="answer-comment">
            <textarea className="answer-textArea"
        placeholder="Comment should be more than 5 words ..." value={this.state.comment} onChange={this.handleCommentChange} ></textarea>
</div>


<div className="answer-rateIt">
    <input ref="rating" type="number" className="rating" data-size="sm"/>
    <br/>
    <button className="buttonRatingSubmit" onClick={this.handleSubmitAnswer}>Submit</button>
    <span id="span-buttonSubmitMsg" style={displayNone}></span>
</div>
<br style={styleClear}/><br/>
</div>

<div id="answer-see-all-comments" style={showCommentCount}><a href={this.props.baseUrl + "Vote/" + this.props.answerTitleURL + "/Comment/" + this.props.id}>see comments (<span id='span-commentCount'>{this.props.commentCount}</span>)</a></div>

<div className="answer-submitMessage">Rating/Comment submitted. Thank You !</div><br/>
</div>
        );
    }
});

//React.render(<TopicAnswers url="/eusvote/topic/topicGet?Filter=AnswersGet&FilterData=1" />, document.getElementById('topicAnswers'));

//<div><a href={"http://eusVille.com/comment/" + this.props.id} >see all comments (<span id='span-commentCount'>{this.props.commentCount}</span>)</a></div>