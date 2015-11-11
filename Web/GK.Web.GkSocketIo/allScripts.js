var bdy = $('body');

/////////////////////////////////////////////////////////////////////////////////////////////////////// SLIDER
if( detectElem( $('.minusMiniSliderHolder') ) )
	$('.minusMiniSliderHolder').minusSlider({ hover:true, speed:500, paging:false, navigation:false, duration:4 });

/////////////////////////////////////////////////////////////////////////////////////////////////////// MOBILE MENU
if( detectElem( $('.mbMbtn') ) )
	$('.mbMbtn').bind('click', function(){
		if( bdy.hasClass('loginMenuReady') ) closeLoginMenu();
		if( bdy.hasClass('mobiMenuReady') )
			closeMobiMenu();
		else 
			cssClass({ 'ID': 'body', 'delay': 100, 'type': 'add', 'cls':['mobiMenuReady', 'mobileMenuOpened'] });
	});
	
if( detectElem( $('.vMbMenu.vail') ) )
	$('.vMbMenu.vail, .leftMenu ul li a').bind('click', closeMobiMenu);

function closeMobiMenu(){
	cssClass({ 'ID': 'body', 'delay': 444, 'type': 'remove', 'cls':['mobileMenuOpened', 'mobiMenuReady'] });
}	

/////////////////////////////////////////////////////////////////////////////////////////////////////// UYE LOGIN

if( detectElem( $('.mLogoutHldr') ) )
	$('.mLogoutHldr').minusDropDown();

if( detectElem( $('.prfbtn') ) )
	$('.prfbtn').bind('click', function(){
		if( bdy.hasClass('mobiMenuReady') ) closeMobiMenu();
		if( bdy.hasClass('loginMenuReady') )
			closeLoginMenu();
		else 
			cssClass({ 'ID': 'body', 'delay': 100, 'type': 'add', 'cls':['loginMenuReady', 'loginMenuOpened'] });
	});

if( detectElem( $('.vLgMenu.vail') ) )
	$('.vLgMenu.vail').bind('click', closeLoginMenu);

function closeLoginMenu(){
	cssClass({ 'ID': 'body', 'delay': 444, 'type': 'remove', 'cls':['loginMenuOpened', 'loginMenuReady'] });
}

/////////////////////////////////////////////////////////////////////////////////////////////////////// STYLER

if( detectElem( $('.gift-select select') ) ) $('.gift-select select').iStyler({wrapper:true});

/////////////////////////////////////////////////////////////////////////////////////////////////////// GLOBAL FUNCTION

function detectElem( ID ){
	return ID.length > 0 ? true : false;
}

function cssClass( o, callback ){
	var ID = $( o['ID'] ), k = o['delay'], type = o['type'], cls;
	if( detectElem( ID ) ){
		if( type == 'add' ){
			cls = o['cls'] || ['ready', 'animate'];
			ID.addClass( cls[ 0 ] ).delay( k ).queue('fx', function(){ $( this ).dequeue().addClass( cls[ 1 ] ); if( callback != undefined ) callback(); });
		}else{
			cls = o['cls'] || ['animate', 'ready'];
			ID.removeClass( cls[ 0 ] ).delay( k ).queue('fx', function(){ $( this ).dequeue().removeClass( cls[ 1 ] ); if( callback != undefined ) callback(); });
		}
	}
}		




