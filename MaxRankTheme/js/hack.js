// Vertical-align: middle.
function middle() {
	var target = ".component"; // Parent block.
	var $parents = $(target);
	$.each($parents, function(index, parent) {
		if (parent.hasChildNodes() == true) {
			var height = parent.clientHeight;
			var child = parent.firstChild;
			while(child != null && child.nodeType == 3){ // Skip TextNodes.
				child = child.nextSibling;
			}
			var marginTop = height - child.clientHeight;
			marginTop /= 2;
			child.setAttribute("style", "margin-top:" + marginTop + "px");
		}
	});
}

// All child block will have equal height.
function height_alignment() {
    var target = '.height-alignment';
    var $children = $(target).children();
    var maxHeight = 0;
    $.each($children, function(index, child) {
        maxHeight = Math.max(maxHeight, child.clientHeight);
    });
    $children.css('height', maxHeight);
}

// Use all hacks.
jQuery(document).ready(function() {
	height_alignment();
	middle();
});