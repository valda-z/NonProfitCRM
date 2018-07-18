
function generateBarcode(elem) {
    var canvas = elem;
    var ctx = canvas.getContext("2d");

    // background
    ctx.rect(0, 0, $(canvas).width(), $(canvas).height());
    ctx.fillStyle = "#ffffff";
    ctx.fill();

    // MAC and SN codes
    var mac = $(elem).data('mac').toString();
    var sn = $(elem).data('sn').toString();

    // MAC settings ------------------------------
    var settings = {
        output: 'canvas',
        bgColor: '#ffffff',
        color: '#000000',
        barWidth: 20,
        barHeight: 400,
        moduleSize: 5,
        posX: 0,
        posY: 290,
        showHRI: false,
        addQuietZone: false
    };
    $(elem).html("").show().barcode(mac, 'code128', settings);

    ctx.font = "250px Conv_FontfabricNexaRegular";

    var macLabel = "MAC: ";
    var macValLen = ctx.measureText(mac).width;
    var macLabelLen = ctx.measureText(macLabel).width;

    var alignLeft = (($(canvas).width() / 2) - ((macValLen + macLabelLen) / 2) + macLabelLen);
    ctx.fillText(mac, alignLeft, 200);

    ctx.font = "250px Conv_FontfabricNexaXBold";
    var alignLeft = (($(canvas).width() / 2) - ((macValLen + macLabelLen) / 2));
    ctx.fillText(macLabel, alignLeft, 200);

    // SN settings -------------------------------
    var settings = {
        output: 'canvas',
        bgColor: '#ffffff',
        color: '#000000',
        barWidth: 20,
        barHeight: 400,
        moduleSize: 50,
        posX: 0,
        posY: 1100,
        showHRI: false,
        addQuietZone: false
    };
    $(elem).html("").show().barcode(sn, 'code128', settings);

    ctx.font = "250px Conv_FontfabricNexaRegular";

    var snLabel = "BBBSN: ";
    var snValLen = ctx.measureText(sn).width;
    var snLabelLen = ctx.measureText(snLabel).width;

    var alignLeft = (($(canvas).width() / 2) - ((snValLen + snLabelLen) / 2) + snLabelLen);
    ctx.fillText(sn, alignLeft, 1010);

    ctx.font = "250px Conv_FontfabricNexaXBold";
    var alignLeft = (($(canvas).width() / 2) - ((snValLen + snLabelLen) / 2));
    ctx.fillText(snLabel, alignLeft, 1010);
}

var loadFonts = ['Conv_FontfabricNexaXBold', 'Conv_FontfabricNexaRegular'];


function redrawBarcodes() {
    WebFont.load({
        custom: {
            families: loadFonts
        },
        fontactive: function (familyName, fvd) {
            var id = loadFonts.indexOf(familyName);
            if (id != -1) {
                loadFonts.splice(id, 1);
            }

            if (loadFonts.length == 0) {
                $(".barcode-target").each(function () {
                    $(this).data('origW', $(this).width());
                    var cropMarks = '<div class="corner corner-1"></div><div class="corner corner-2"></div><div class="corner corner-3"></div><div class="corner corner-4"></div>';
                    $(".barcode-target").parent().append(cropMarks);
                    generateBarcode(this);
                });
            }
        }
    });
};