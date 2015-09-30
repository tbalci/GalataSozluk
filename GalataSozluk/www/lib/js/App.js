//Sabitler
var InternetBaglantiTestMesaj = "İnternet Bağlantısı Yok.Lütfen İnternet Bağlantınızı Açınız.";
var UygulamaAd = "Sözlük";
//Sabitler Bitişi

String.prototype.startsWith = function (needle) {
    var Index = this.indexOf(needle);
    if (Index == 0) {
        return true;
    }
    else {
        return false
    }
};

String.prototype.replaceAll = function (token, newToken, ignoreCase) {
    var _token;
    var str = this + "";
    var i = -1;

    if (typeof token === "string") {
        if (ignoreCase) {
            _token = token.toLowerCase();

            while ((
                i = str.toLowerCase().indexOf(
                    token, i >= 0 ? i + newToken.length : 0
                )) !== -1
            ) {
                str = str.substring(0, i) +
                    newToken +
                    str.substring(i + token.length);
            }
        } else {
            return this.split(token).join(newToken);
        }
    }
    return str;
};

function supports_html5_storage() {
    try {
        return 'localStorage' in window && window['localStorage'] !== null;
    } catch (e) {
        return false;
    }
}
function supports_json() {
    try {
        return 'JSON' in window && window['JSON'] !== null;
    } catch (e) {
        return false;
    }
}

function ResmiLocalStoraKaydet(StoreAd, ResimUrl) {
    var elephant = new Image();
    elephant.src = ResimUrl;
    var imgCanvas = document.createElement("canvas"),
    imgContext = imgCanvas.getContext("2d");
    imgCanvas.width = elephant.width;
    imgCanvas.height = elephant.height;
    imgContext.drawImage(elephant, 0, 0, elephant.width, elephant.height);
    var imgAsDataURL = imgCanvas.toDataURL("image/png");
    localStorage.setItem(StoreAd, imgAsDataURL);
}

ko.observableArray.fn.pushAll = function (valuesToPush) {
    var underlyingArray = this();
    this.valueWillMutate();
    ko.utils.arrayPushAll(underlyingArray, valuesToPush);
    this.valueHasMutated();
    return this;
};

function SayfaAc(Sayfa, AcilmaSekli) {
    if (AppModel.ClientLisansKontrol() == true) {
        $.mobile.changePage($(Sayfa), { dataUrl: "/", transition: AcilmaSekli, changeHash: true });
    }
    else {
        if (Sayfa != '#Anasayfa') {
            SayfaAc("#Anasayfa", "none");
        }
        else {
            $.mobile.changePage($(Sayfa), { dataUrl: "/", transition: AcilmaSekli, changeHash: true });
        }
    }
}

function MesajGoster(Baslik, Icerik, AltBilgi, DonusSayfa) {
    $("#Mesaj-Baslik").html(Baslik);
    $("#Mesaj-Icerik").html(Icerik);
    $("#Mesaj-AltBilgi").html(AltBilgi);
    $.mobile.changePage($("#MesajPenceresi"), { dataUrl: "/", transition: "none", changeHash: true });
    $('#Mesaj_Kapat').click(function () {
        MesajPenceresiKapat(DonusSayfa);
    })
};

function MesajPenceresiKapat(DonusSayfa) {
    SayfaAc(DonusSayfa, 'none');
};

function KontrolGizleGoster(Kontrol, Durum) {
    if (Durum) {
        $(Kontrol).css({ "display": "block" });
    }
    else {
        $(Kontrol).css({ "display": "none" });
    }
};

function InternetBaglantisiKontrolEt() {
    if (!window.navigator.onLine) {
        AppModel.ToastMesajVer('İnternet Durumu', InternetBaglantiTestMesaj, 'warning', 10000, true);
        return;
    }
};

var db = {};

var AppModel = {
    Uygulama: {
        Ad: ko.observable('Sözlük'),
        FooterBilgi: ko.observable('Fransızca,Türkçe,İnilizce olarak 11.000 kelime'),
        Logo: ko.observable('data:image/png;base64,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')
    },
    ToastMesajVer: function (Baslik, Mesaj, Icon, Delay, CloseButton) {
        $.amaran({
            content: {
                title: Baslik,
                message: Mesaj,
                info: '',
                icon: 'fa fa-' + Icon,
            },
            theme: 'awesome ok',
            inEffect: 'slideTop',
            outEffect: 'slideRight',
            delay: Delay,
            closeButton: CloseButton,
            clearAll: true,
        });
    },
    LoadingPenceresiAc: function (Mesaj) {
        $.mobile.loading("show", {
            text: Mesaj,
            textVisible: true,
        });
    },
    LoadingPenceresiKapat: function () {
        $.mobile.loading("hide");
    },
    AnaMenuOlustur: function () {
        var self = $('.AnaMenuGit')
        self.attr("data-iconpos", "notext");
        self.attr("href", "#Anasayfa");
        self.attr("data-role", "button");
        self.attr("data-icon", "home");
        self.attr("data-inline", "true");
    },
    UygulamaAdBilgisiOlustur: function () {
        var Nesne = this;
        $('.PnlUygulamaAd').html(
            '<center><div style="float:left;"><img border="0" height="65px" src=' + AppModel.Uygulama.Logo() + ' alt=' + AppModel.Uygulama.Ad() + ' style="display:inline" ; /></div><div style="float:left;margin-top:25px;margin-left:5px;"> ' + AppModel.Uygulama.Ad() + '</div></center>'
            );
    },
    UygulamaFooterOlustur: function () {
        var Nesne = this;
        $('.PnlFooterTxt').html('<span style="font-size:12px;margin-left:5px">' + AppModel.Uygulama.FooterBilgi() + '</span>');
    },
    Dil: ko.observable(1),
    Sonuclar: ko.observableArray([]),
    SaSonuclar: ko.observableArray([]),
    SecilenSonuclar: ko.observableArray([]),
    SonucSec: function (id) {
        var sonuc;
        sonuc =
            _(AppModel.Sonuclar()).chain().where({ id: id }).value();
        AppModel.SecilenSonuclar.removeAll()
        AppModel.SecilenSonuclar.pushAll(sonuc);
    },
    Ara: function (Txt) {
        Txt = Txt.toLowerCase();
        var sonuc;
        var sonucsa;
        if (AppModel.Dil == 1) {
            sonuc = _.filter(db.Kelimeler, function (obj) {
                if (obj.Turkce.toLowerCase() == Txt) {
                    return obj;
                }
            });
        }
        if (AppModel.Dil == 2) {
            sonuc = _.filter(db.Kelimeler, function (obj) {
                if (obj.IngilizceF.toLowerCase() == Txt) {
                    return obj;
                }
            });
        }
        if (AppModel.Dil == 3) {
            sonuc = _.filter(db.Kelimeler, function (obj) {
                if (obj.FransizcaF.toLowerCase() == Txt) {
                    return obj;
                }
            });
        }
        if (sonuc != null) {
            if (sonuc.length > 0 && (sonuc[0].Sa != '0')) {
                console.log(sonuc[0].Sa);
                sonucsa = _.where(db.Kelimeler, { Sa: sonuc[0].Sa });
                sonucsa = _.filter(sonucsa, function (obj) {
                    if (obj.Id != sonuc[0].Id) {
                        return obj;
                    }
                });
            }
            else {
                sonucsa = sonuc;
                console.log(sonucsa);
            }
        }

        AppModel.Sonuclar.removeAll()
        AppModel.Sonuclar.pushAll(sonuc);

        AppModel.SaSonuclar.removeAll()
        AppModel.SaSonuclar.pushAll(sonucsa);
    },
    VeriTabaniniHazirla: function () {
        AppModel.LoadingPenceresiAc('Veriler Hazırlanıyor..!');
        db = VerileriYukle();
    }
}
$(document).ready(function () {
    ko.applyBindings(AppModel, document.getElementById("#Anasayfa"));
    var MaxLength = $sutunTxtAra.parent().parent().parent().width();
    var DegerW = MaxLength - 132;
    $sutunTxtAra.parent().attr("style", "width:" + DegerW + "px");
});