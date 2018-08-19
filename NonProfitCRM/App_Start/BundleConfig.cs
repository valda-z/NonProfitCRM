/*
* MIT License
* 
* Copyright (c) 2015 - present valda-z
* 
* All rights reserved.
*
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in all
* copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
* SOFTWARE.
*/

﻿using System.Web;
using System.Web.Optimization;

namespace NonProfitCRM
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Content/sing/vendor/libs-js").Include(
                        "~/Content/sing/vendor/jquery/dist/jquery.min.js",
                        "~/Content/sing/vendor/jquery-pjax/jquery.pjax.js",
                        "~/Content/sing/vendor/bootstrap-sass/assets/javascripts/bootstrap/transition.js",
                        "~/Content/sing/vendor/bootstrap-sass/assets/javascripts/bootstrap/collapse.js",
                        "~/Content/sing/vendor/bootstrap-sass/assets/javascripts/bootstrap/dropdown.js",
                        "~/Content/sing/vendor/bootstrap-sass/assets/javascripts/bootstrap/button.js",
                        "~/Content/sing/vendor/bootstrap-sass/assets/javascripts/bootstrap/tooltip.js",
                        "~/Content/sing/vendor/bootstrap-sass/assets/javascripts/bootstrap/alert.js",
                        "~/Content/sing/vendor/slimScroll/jquery.slimscroll.min.js",
                        "~/Content/sing/vendor/widgster/widgster.js",
                        "~/Content/sing/vendor/pace.js/pace.min.js",
                        "~/Content/sing/vendor/select2/select2.js",
                        "~/Content/sing/vendor/jquery-touchswipe/jquery.touchSwipe.js",
                        "~/Content/sing/vendor/jquery-touchswipe/jquery.touchSwipe.js"
                        ));

            bundles.Add(new ScriptBundle("~/Content/sing/vendor/form-js").Include(
                        "~/Content/sing/vendor/parsleyjs/dist/parsley.min.js",
                        "~/Content/sing/vendor/bootstrap-sass/assets/javascripts/bootstrap/tooltip.js",
                        "~/Content/sing/vendor/bootstrap-sass/assets/javascripts/bootstrap/modal.js",
                        "~/Content/sing/vendor/bootstrap-select/dist/js/bootstrap-select.min.js",
                        "~/Content/sing/vendor/jquery-autosize/jquery.autosize.min.js",
                        "~/Content/sing/vendor/bootstrap3-wysihtml5/lib/js/wysihtml5-0.3.0.min.js",
                        "~/Content/sing/vendor/bootstrap3-wysihtml5/src/bootstrap3-wysihtml5.js",
                        "~/Content/sing/vendor/switchery/dist/switchery.min.js",
                        "~/Content/sing/vendor/moment/min/moment.min.js",
                        "~/Content/sing/vendor/eonasdan-bootstrap-datetimepicker/build/js/bootstrap-datetimepicker.min.js",
                        "~/Content/sing/vendor/mjolnic-bootstrap-colorpicker/dist/js/bootstrap-colorpicker.min.js",
                        "~/Content/sing/vendor/jasny-bootstrap/js/inputmask.js",
                        "~/Content/sing/vendor/jasny-bootstrap/js/fileinput.js",
                        "~/Content/sing/vendor/holderjs/holder.js",
                        "~/Content/sing/vendor/dropzone/dist/min/dropzone.min.js",
                        "~/Content/sing/vendor/markdown/lib/markdown.js",
                        "~/Content/sing/vendor/bootstrap-markdown/js/bootstrap-markdown.js",
                        "~/Content/sing/vendor/seiyria-bootstrap-slider/dist/bootstrap-slider.min.js",
                        "~/Content/sing/vendor/bootstrap-sass/assets/javascripts/bootstrap/tab.js"
                        ));

            bundles.Add(new ScriptBundle("~/Content/sing/js/apps-js").Include(
                        "~/Content/sing/js/settings.js",
                        //"~/Content/sing/vendor/jvectormap/jquery-jvectormap-1.2.2.min.js",
                        //"~/Content/sing/vendor/jvectormap-world/index.js",
                        "~/Content/sing/vendor/bootstrap-tagsinput/typeahead.bundle.js",
                        "~/Content/sing/vendor/bootstrap-tagsinput/bootstrap-tagsinput.min.js",
                        "~/Content/sing/js/app.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jstz").Include(
                        "~/Scripts/jstz-1.0.4.min.js"
                        ));

            bundles.Add(new ScriptBundle("~/Content/sing/vendor/fullcalendar").Include(
                        "~/Content/sing/vendor/fullcalendar/fullcalendar.js"
                        ));

            bundles.Add(new ScriptBundle("~/Content/sing/vendor/spec-js").Include(
                        "~/Content/sing/vendor/underscore/underscore.js",
                        "~/Content/sing/vendor/jquery.sparkline/index.js",
                        "~/Content/sing/vendor/d3/d3.min.js",
                        "~/Content/sing/vendor/rickshaw/rickshaw.min.js",
                        "~/Content/sing/vendor/raphael/raphael-min.js",
                        "~/Content/sing/vendor/jQuery-Mapael/js/jquery.mapael.js",
                        "~/Content/sing/vendor/jQuery-Mapael/js/maps/usa_states.js",
                        "~/Content/sing/vendor/jQuery-Mapael/js/maps/world_countries.js",
                        "~/Content/sing/vendor/bootstrap-sass/assets/javascripts/bootstrap/popover.js",
                        "~/Content/sing/vendor/bootstrap_calendar/bootstrap_calendar/js/bootstrap_calendar.min.js",
                        "~/Content/sing/vendor/jquery-animateNumber/jquery.animateNumber.min.js"
                        ));

            bundles.Add(new StyleBundle("~/Content/sing/css/sing").Include(
                        "~/Content/sing/css/application.min.css",
                        "~/Content/sing/vendor/bootstrap-tagsinput/bootstrap-tagsinput.css"
                        ));

            bundles.Add(new StyleBundle("~/Content/custom/css").Include(
                        "~/Content/Site.css"));

            bundles.Add(new StyleBundle("~/Content/barcode/css").Include(
                        "~/Content/BarCode/styles.css"));

            bundles.Add(new ScriptBundle("~/Content/barcode/js").Include(
                        "~/Content/BarCode/jquery-barcode.js",
                        "~/Content/BarCode/script.js",
                        "~/Content/BarCode/webfontloader.js"
                        ));
        }
    }
}