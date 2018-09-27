$(function(){
    function pageLoad(){
        $('.widget').widgster();
        //init parsley for pjax requests
        $('#validation-form').parsley({
            excluded:
                // Defaults
                'input[type=radio],' +
                'input[type=button],' +
                'input[type=submit],' +
                'input[type=reset],' +
                'input[type=hidden],' +
                '[disabled],' +
                ':hidden,' +
                // -- Additional attributes to look --
                '[data-parsley-disabled],' +  // Exclude specific input/select/radio/checkbox/etc
                '[data-parsley-disabled] *'   // Exclude all nesting inputs/selects/radios/checkboxes/etc
        });
    }
    pageLoad();
    SingApp.onPageLoad(pageLoad);
});