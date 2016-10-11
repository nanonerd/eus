var CommentBox = React.createClass({
	getInitialState: function(){
		//var self = this;	// Need to declare here, not in e.g., componentDidMount
		//var x = $('head');
		return {data: this.props.initialComments};
	},
	loadCommentsFromServer: function(){
		//var url = this.props.url + this.props.answerID;
		var url = this.props.baseURL + "Vote/" + this.props.answerTitleURL + "/Comment/Get/" + this.props.answerID;
		var result = eusAPI.get(url);
		this.setState({ data: result});
	},
	handleCommentSubmit: function(comment){
		// submit to the server and refresh the list
		// Create json array with UserID, AnswerID, and Rating

		var comments = this.state.data;
		var newComments = comments.concat([comment]);
		this.setState({data: newComments});

		var obj = {};

		obj["UserID"] = this.props.userID;
		obj["AnswerID"] = this.props.answerID;   // $(this).data('id');

		obj["Comment"] = comment.Text;

		// Create json string
		var data = JSON.stringify(obj);

		// Create json string with Filter to send to API
		var jsonString = JSON.stringify({ "Filter": "CommentPost", "FilterData": data });

		var submitURL = this.props.baseURL + "Vote/" + this.props.answerTitleURL + "/Comment/Post/" + this.props.answerID;

		var result = eusAPI.post(submitURL, jsonString);
        this.loadCommentsFromServer();
	},
	componentDidMount: function(){
		// Create URL so user can return to topic page
		var answerID = this.props.answerID;
		var answerTitle = this.props.answerTitle;
		var topicID = this.props.topicID;
		var topicTitleURL = this.props.topicTitleURL;
		var topicTitle = this.props.topicTitle;
		var returnURL = this.props.baseURL + "vote/" + topicTitleURL + "/" + topicID;

		console.log("returnURL: " + returnURL);
		console.log("userID: " + this.props.userID);


			//$("#topicReturnURL").html("<a href=" + self.returnURL + "> Back to topic: " + self.topicTitle + "</a>");

        if (this.props.userID == 0){
            $("#postButton").hide();
            //document.getElementById('postButton').style.display = 'none';
        }
        else
        {
            $("#notLoggedIn").hide();
            //document.getElementById(notLoggedIn).style.display = 'none';
        }

		$("#topicReturnURL").html("<a href=" + returnURL + "> Back to topic: " + topicTitle + "</a>");



		$('#answerTitle').text(answerTitle);   // Set comment title
		//this.loadCommentsFromServer();
		//window.setInterval(this.loadCommentsFromServer, this.props.pollInterval);
	},
	render: function(){
		return (
			<div>
			<Header />
			<div className="commentBox">

				<div className="commentBoxLeft">
					hello world thello world
				</div>
				<div className="commentBoxRight">
					<CommentList data={this.state.data} />
					<CommentForm onCommentSubmit={this.handleCommentSubmit} userID={this.props.userID}/>
				</div>
			</div>
				</div>
		);
	}
});

// Header that can be customized for each comment page
var Header = React.createClass({
	render: function(){
		return (
			<div>
				<br/>
				<div id="topicReturnURL">
				</div>

				<div>
					<h2><span id='answerTitle'></span></h2>
					[Rate Button]  eusScore: <span id="span-eusScore"></span>
				</div><br/>
			</div>
		);
	}
});

// Comment container to hold list of comments
var CommentList = React.createClass({
	render: function(){

		var commentNodes = this.props.data.map(function(comment){
			return(
				<Comment author={comment.Author} text={comment.CommentText} commentDate={comment.LongDate} commentTime={comment.ShortTime}>
				</Comment>
			);
		});
		return (
			<div className="commentList">
				{commentNodes}
			</div>
		);
	}
});

var Comment = React.createClass({
	render: function(){
		var auth = "test author"; // this.props.author.toString();
		var floatLeft={float: 'left'};

		//var converter = new Showdown.converter();
		//var rawMarkup = converter.makeHtml(this.props.children.toString());
		return (
			<div className="comment">
				<div>
					<span>{this.props.author}</span>
					&nbsp;&nbsp;&nbsp;
					<span>{this.props.commentDate}</span>
					&nbsp;&nbsp;&nbsp;
					<span>{this.props.commentTime}</span><br/><br/>
				</div>
				<div>
					{this.props.text}
				</div><br/>
			</div>
		);
	}
});

var CommentForm = React.createClass({
	getInitialState: function(){
		return {text: ''};
	},
	handleTextChange: function(e){
		this.setState({text: e.target.value});
	},

	handleSubmit: function(e) {
		// TODO: If not logged in, the Submit button should be disabled. But do a check anyways to make sure that user is logged in.

		e.preventDefault();
    	var text = this.state.text.trim();

		if (!text){
			return;
		}
		// todo: send request to the server
		this.props.onCommentSubmit({Text: text});

		// reset the fields to blanks
		this.setState({text: ''});
		return;
	},
	render: function(){

		console.log("userID form: " + this.props.userID);




		return (

			<form className="commentForm" onSubmit={this.handleSubmit}>
				<input type="text" placeholder="Type comment" value={this.state.text} onChange={this.handleTextChange}/>
				<input id="postButton" type="submit" value="Post" />
				<span id="notLoggedIn">Log in to post comment</span>
			</form>
		);
	}
});