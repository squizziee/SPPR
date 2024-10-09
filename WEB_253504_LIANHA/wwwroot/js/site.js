setPagerListener();

function setPagerListener() {
	$('span.page-link').unbind();
	$('span.page-link').click(function () {
		var url = $(this).attr('href');  // Get the URL from the data attribute

		// Make the AJAX GET request
		$.ajax({
			url: url,
			type: 'GET',
			success: function (data) {
				$('#partial_conatiner').html(data)
				//console.log(data)
				setPagerListener()
			},
			error: function (xhr, status, error) {
				console.log('Query: ' + url)
				console.log('Error: ' + error);
			}
		});

		$(document).ajaxComplete(setPagerListener)
	});
}