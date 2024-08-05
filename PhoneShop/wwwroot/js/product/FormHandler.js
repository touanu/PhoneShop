// Xử lý ảnh sản phẩm
$('#productImages').on('change', function () {
    var dvPreview = $("#dvPreview");

    if (typeof (FileReader) == "undefined") {
        var unsupportedBrowser = $(
            "<p id='imageHint' style='color:red'>Trình duyệt này không hỗ trợ HTML5 FileReader,</p>" +
            "<p id='imageHint' style='color:red'>không thể hiển thị ảnh xem trước.</p>"
        );

        $("#imageHint").remove();
        dvPreview.append(unsupportedBrowser);
        return;
    }

    var regex = /^([a-zA-Z0-9\s_\\.\-:])+(.jpg|.jpeg|.gif|.png|.bmp)$/;
    var files = $(this)[0].files

    // Lấy ảnh được upload vào #productImages, đọc chúng và
    // hiện thị ảnh với src là base64
    $(files).each(function () {
        var file = $(this);

        // Kiểm tra xem tên file có phải là file ảnh hợp lệ không
        if (!regex.test(file[0].name.toLowerCase())) {
            alert(file[0].name + " không phải là file ảnh.");
            return;
        }

        var reader = new FileReader();
        reader.onload = function (e) {
            var index = $("#dvPreview img").length + 1;
            var img = $(
                "<img class='img_" + index + "'/>"
            );
            var deleteButton = $(
                "<span id='delete_img_" + index + "'>" +
                "<a onclick='RemoveProductImage(" + index + ")'>Xóa</a>" +
                "</span> <br class='img_" + index + "' />"
            );

            img.attr("style", "height:250px;width: 250px");
            img.attr("src", e.target.result);

            dvPreview.append(img);
            dvPreview.append(deleteButton);
        };
        reader.readAsDataURL(file[0]);
    });
});
function openFileOption() {
    $("#productImages").click();
}
function RemoveProductImage(index) {
    $("#dvPreview .img_" + index).remove();
    $("#dvPreview #delete_img_" + index).remove();
}

// Xử lý thuộc tính của sản phẩm
function ChangeAttributeTitle(index) {
    var attributeName = $(`#attributeName_${index}`)[0].value;
    $(`#attribute_title_${index}`)[0].textContent = "Thuộc tính: " + attributeName;
}
function AddNewAttribute() {
    var index = $(".productAttribute").length;
    var html = `
                <div class="card card-info card-outline productAttribute" id="attribute_${index}">
                    <div class="card-header">
                        <h3 class="card-title" id="attribute_title_${index}">Thuộc tính</h3>

                        <div class="card-tools">
                            <button type="button" class="btn btn-tool" title="Xoá" onclick="RemoveAttribute(${index});">
                                <i class="fas fa-times"></i>
                            </button>
                            <button type="button" class="btn btn-tool" data-card-widget="collapse" title="Collapse">
                                <i class="fas fa-minus"></i>
                            </button>
                        </div>
                    </div>
                    <div class="card-body">
                        <div class="form-group">
                            <label for="attributeName_${index}">Tên thuộc tính</label>
                            <input type="text" id="attributeName_${index}" class="form-control" onchange="ChangeAttributeTitle(${index});" required>
                        </div>
                        <div class="form-group" id="productAttributeValues_${index}">
                            <div class="container p-1 productAttributeValue" id="productAttributeValue_${index}_0">
                                <div class="row p-1">
                                    <div class="col">
                                        <label for="attributeValueName_${index}_0">Tên giá trị</label>
                                        <input type="text" id="attributeValueName_${index}_0" class="form-control" required>
                                    </div>
                                    <div class="col align-content-end">
                                        <a style="cursor:pointer; display: none;" onclick="RemoveAttributeValue(${index},0)">
                                            <i class="fa fa-plus-circle"></i>
                                            Xoá
                                        </a>
                                    </div>
                                </div>
                                <div class="row p-1">
                                    <div class="col">
                                        <label for="quantity_${index}_0">Số lượng</label>
                                        <input type="number" id="quantity_${index}_0" class="form-control" placeholder="1" required>
                                    </div>
                                    <div class="col">
                                        <label for="price_${index}_0">Giá</label>
                                        <input type="number" id="price_${index}_0" class="form-control" placeholder="${index}${index}.${index}${index}" required>
                                    </div>
                                    <div class="col">
                                        <label for="priceSale_${index}_0">Giá sale</label>
                                        <input type="number" id="priceSale_${index}_0" class="form-control" placeholder="${index}${index}.${index}${index}" required>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                                <a style="cursor:pointer;" onclick="AddNewAttributeValue(${index})">
                                <i class="fa fa-plus-circle"></i>
                                Thêm giá trị mới
                            </a>
                        </div>
                    </div>
                </div>
            `

    $("#productAttributeContainer").append(
        $(html)
    );
}
function AddNewAttributeValue(attributeIndex) {
    var index = $(`#productAttributeValues_${attributeIndex} .productAttributeValue`).length;
    var container = $("#productAttributeValues_" + attributeIndex);

    var html = `
                <div class="container p-1 productAttributeValue" id="productAttributeValue_${attributeIndex}_${index}">
                    <div class="row p-1">
                        <div class="col">
                            <label for="attributeValueName_${attributeIndex}_${index}">Tên giá trị</label>
                            <input type="text" id="attributeValueName_${attributeIndex}_${index}" class="form-control" required>
                        </div>
                        <div class="col align-content-end">
                            <a style="cursor:pointer;" onclick="RemoveAttributeValue(${attributeIndex}, ${index})">
                                <i class="fas fa-times-circle"></i>
                                Xoá
                            </a>
                        </div>
                    </div>
                    <div class="row p-1">
                        <div class="col">
                            <label for="quantity_${attributeIndex}_${index}">Số lượng</label>
                            <input type="number" id="quantity_${attributeIndex}_${index}" class="form-control" placeholder="1" required>
                        </div>
                        <div class="col">
                            <label for="price_${attributeIndex}_${index}">Giá</label>
                            <input type="number" id="price_${attributeIndex}_${index}" class="form-control" placeholder="00.00" required>
                        </div>
                        <div class="col">
                            <label for="priceSale_${attributeIndex}_${index}">Giá sale</label>
                            <input type="number" id="priceSale_${attributeIndex}_${index}" class="form-control" placeholder="00.00" required>
                        </div>
                    </div>
                </div>
            `;

    container.append(
        $(html)
    );
}
function RemoveAttribute(index) {
    $("#attribute_" + index).remove();
}
function RemoveAttributeValue(attributeIndex, valueIndex) {
    $("#productAttributeValue_" + attributeIndex + "_" + valueIndex).remove();
}

// Gửi dữ liệu về server
function SaveData(postAction) {
    var isValidated = true;
    $("#product input:not('#productImages')").each(function (index, value) {
        if (!$(this).val()) {

            isValidated = false;
            return false;
        }
    });

    if (!isValidated) {
        alert("Vui lòng điền đầy đủ thông tin trước khi thêm vào cơ sở dữ liệu!");
        return;
    }

    var name = $("#productName").val();
    var categoryId = $("#productCategory").val();
    var brandId = $("#productBrand").val();
    var description = $("#productDescription").val();

    var lstImageSrc = "";
    // Lấy tất cả cả thẻ img trong dvPreview
    $("#dvPreview img").each(function (index, item) {
        var imageSource = item.src;
        
        if (imageSource == null || imageSource == "") {
            return;
        }

        if (imageSource.endsWith(".png")) {
            imageSource = imageSource.split("/");
            var image = imageSource[imageSource.length - 1];
        }
        else {
            // Cắt bỏ đi data:image/jpeg;base64,
            var image = imageSource.split(',')[1];
        }
        
        lstImageSrc += image + "_";
    });
    if (lstImageSrc == null || lstImageSrc.length <= 0) {
        alert("Vui lòng thêm ít nhất một ảnh mô tả sản phẩm.");
        return;
    }
    // Cắt bỏ ký tự "_" ở cuối cùng
    lstImageSrc = lstImageSrc.substring(0, lstImageSrc.length - 1);

    var attributes = GetAllAttributes();

    var url = new URL(window.location.href);
    var productId = url.searchParams.get("id");

    var param = {
        ProductID: productId,
        ProductName: name,
        CategoryID: categoryId,
        BrandID: brandId,
        Attributes: attributes,
        Images: lstImageSrc,
        ProductDescription: description,
    };

    console.log(param);

    $.ajax({
        type: 'POST',
        url: "/Product/" + postAction,
        data: param,
        async: true,
        dataType: "json",
        success: function (rs) {
            alert(rs.returnMsg);
            if (rs.returnCode > 0) {
                window.location.replace("/Product?success=true");
            }
        },
        error(rs) {
            console.log(JSON.stringify(rs));
        }
    });
}
function GetAllAttributes() {
    var attributes = "";

    $("#productAttributeContainer .productAttribute").each(function (index, value) {
        var AttributeName = $("#attributeName_" + index).val();
        var AttributeValues = GetAllAttributeValues(index);
        attributes += AttributeName + '_' + AttributeValues + '|';
    });

    attributes = attributes != "" ? attributes.substr(0, attributes.length - 1) : attributes;

    // Kiểm tra console để có cấu trúc dữ liệu trả về
    console.log("attributes:" + attributes);
    return attributes;
}
function GetAllAttributeValues(attributeIndex) {
    var attributeValues = "";

    $(`#productAttributeValues_${attributeIndex} .container`).each(function (index, value) {
        var ValueName = $("#attributeValueName_" + attributeIndex + "_" + index).val();
        var ValueQuantity = $("#quantity_" + attributeIndex + "_" + index).val();
        var ValuePrice = $("#price_" + attributeIndex + "_" + index).val();
        var ValueSalePrice = $("#priceSale_" + attributeIndex + "_" + index).val();
        attributeValues += ValueName + ","
            + ValueQuantity + ","
            + ValuePrice + ","
            + ValueSalePrice + "_";
    });

    attributeValues = attributeValues != "" ? attributeValues.substr(0, attributeValues.length - 1) : attributeValues;

    // Kiểm tra console để có cấu trúc dữ liệu trả về
    console.log("attributeValues:" + attributeValues, "index:", attributeIndex);
    return attributeValues;
}