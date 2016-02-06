﻿var TrackerListPanelRow = React.createClass({
	handleChange: function(event){
		this.props.handleChange(event);
	},
	handleSearchClick: function(){
		this.props.handleSearchClick(this.props.tracker);
	},
	render: function () {
		return(
			<tr>
				<td>{this.props.index + 1}</td>
				<td>{this.props.tracker.name}</td>
				<td><input type='checkbox' value={this.props.tracker.id} onChange={this.handleChange}></input></td>
				<td><button className='btn btn-default btn-xs' onClick={this.handleSearchClick}><span className='glyphicon glyphicon-search'></span></button></td>
			</tr>
		)
	}
});

var TrackerListPanel = React.createClass({
	getDefaultProps: function () {
		return {
			initialPos: {x: 200, y: 200}
		}
	},
	getInitialState: function () {
		return {
			pos: this.props.initialPos,
			dragging: false,
			rel: null
		}
	},
	componentDidUpdate: function (props, state) {
		if (this.state.dragging && !state.dragging) {
			document.addEventListener('mousemove', this.onMouseMove)
			document.addEventListener('mouseup', this.onMouseUp)
		} else if (!this.state.dragging && state.dragging) {
			document.removeEventListener('mousemove', this.onMouseMove)
			document.removeEventListener('mouseup', this.onMouseUp)
		}
	},
	

	// calculate relative position to the mouse and set dragging=true
	onMouseDown: function (e) {
		// only left mouse button
		if (e.button !== 0) return
		var pos = $(ReactDOM.findDOMNode(this)).position()
		this.setState({
		  dragging: true,
			rel: {
				x: e.pageX - pos.left,
				y: e.pageY - pos.top
			}
		})
		e.stopPropagation()
		e.preventDefault()
	},
	onMouseUp: function (e) {
		this.setState({dragging: false})
		e.stopPropagation();
		e.preventDefault();
	},
	onMouseMove: function (e) {
		if (!this.state.dragging) return
		var newX = e.pageX - this.state.rel.x;
		var newY = e.pageY - this.state.rel.y;
		
		this.setState({
			pos: {
				x: newX,
				y: newY
			}
		})
		e.stopPropagation();
		e.preventDefault();
	},
	handleChange: function (event){
		var tracker;
		var trackers = this.props.trackers;
		
		for(var i = 0; i < trackers.length; i++){
			if(trackers[i].id == event.target.value){
				tracker = trackers[i];
				break;
			}
		}
		this.props.changeSelection(tracker);
	},
	handleSearchClick: function(tracker) {
		this.props.searchTracker(tracker)
	},
	render: function(){
		return(
			<div className='panel panel-default' onMouseDown={this.onMouseDown} style={{width: 250 + 'px', height: 300 + 'px', position: 'absolute', zIndex: 2, left: this.state.pos.x + 'px', top: this.state.pos.y + 'px'}}>
				<div className='panel-heading'>Трекеры</div>
				<div className='panel-body pre-scrollable' style={{height: 250 + 'px'}}>
					<table className='table table-bordered'>
						<tbody>
							{this.props.trackers.map(function(tracker, index) {
								return (
									<TrackerListPanelRow key={index} tracker={tracker} index={index} handleChange={this.handleChange} handleSearchClick={this.handleSearchClick}/>
								);
							}.bind(this))}
						</tbody>
					</table>
				</div>
			</div>
		)
	}
});