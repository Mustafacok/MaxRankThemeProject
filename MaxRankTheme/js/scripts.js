// Twitter
jQuery(function($){
    $(".tm_tweet").tweet({
        username: "html5",
        join_text: "auto",
        count: 2,
        auto_join_text_default: "we said,", 
        auto_join_text_ed: "we",
        auto_join_text_ing: "we were",
        auto_join_text_reply: "we replied to",
        auto_join_text_url: "we were checking out",
        loading_text: "loading tweets..."
    });
});

// Bootstrap Carousel
jQuery('.carousel').carousel({
    pause: "hover",
    interval: 5000
});

// Mobile Menu
jQuery(function(){
    jQuery('.sf-menu').mobileMenu();
});