$(function () {
    $("#cbTumu").click(function () {
        $('input:checkbox').not(this).prop('checked', this.checked);
        SecimBilgisi();
    });
    $(".cbTek").click(function () {
        SecimBilgisi();
    });
    $("#btnAra").click(function () {
        window.location.href = "/yonet/kategori?q=" + $("#text").val();
    });

    $("#spnElemanBilgisi").html("@(elemanblg)");
});
function SecimBilgisi() {
    var seciliolan = $('[name="cbTek"]:checked').length;
    $(".spEleman").html(seciliolan);
    if (seciliolan == 0) {
        $("#btnSil1").show();
        $("#btnSil2").hide();
    }
    else {
        $("#btnSil1").hide();
        $("#btnSil2").show();
    }
    var tumu = $('[name="cbTek"]').length;
    if (tumu == seciliolan) {
        $("#cbTumu").prop("checked", true);
    }
    else {
        $("#cbTumu").prop("checked", false);
    }

    $("#cbSecili").val($('[name="cbTek"]:checked').map(function () { return this.value; }).get().join(','));
}
$(function () {
    $("#text").keyup(function () {
        var sayfa = $(this).data("sayfaadi");
        var textVal = $(this).val();
        arama(sayfa, textVal);
    });
    function arama(sayfaadi, textVal) {
        $("#detayyukle").html("");
        $.ajax({
            url: "/yonet/" +sayfaadi+ "/IndexDetay",
            type: "get",
            data: { term: textVal },
            success: function (data) {
                $("#detayyukle").append(data);
            }
        })
    }
});

//$(function () {
//    $("#text").autocomplete({
//        source: function (request, response) {
//            $("#detayyukle").html("");
//            $.ajax({
//                url: "/yonet/kategori/indexdetay",
//                type: "get",
//                data: { term: request.term },
//                success: function (data) {
//                    $("#detayyukle").append(data);
//                }
//            })
//        },
//        minLength: 1,
//        messages: {
//            noResults: "", results: ""
//        },
//        select: function (event, ui) {
//            window.location.href = "/yonet/kategori/detay/" + ui.item.value;
//        }
//    });
//});