$(function(){
	window.scrollTo(0, 0);
	var secKill = new SecKill();
	secKill.initFn(); 	
	secKill.progress();
});
var SecKill = function(){
	this.oHeader = document.querySelector('#topfixed') ;
	this.m_common_header = document.querySelector('#m_common_header') ;
	this.oAd = document.querySelector('p.big-ad-column') ;
	this.seckillSecond=global_time_remain;
	this.timer= null;
	this.seckillRoundNo = document.querySelector('#seckillRoundNo') ;
	this.seckillTitle = document.querySelector('#seckillTile') ;
	this.seckillBuyText = document.querySelector('#seckillBuyTxt') ;
	this.staticTxtEnd = document.querySelector('#staticTxtEnd') ;
	this.bannerAdsA = document.querySelector('#banner-ads-a');
	this.bannerAdsImg = document.querySelector('#banner-ads-img');
	//constant
	this.constantData = {titleStatusWill:'即将开场',
						titleStatusBeing:'秒杀中',
						infoMsgWillSeckill:'秒杀预告',
						infoMsgSeckilling:'秒杀中',
						infoMsgBegin:'距开始',
						infoMsgEnd:'距结束',
						buyText:'秒杀中 先下单先得哦',
						willBuyText:'即将开场 先下单先得哦'
					}
};
var parseSeckillList = function(data) {
	var content = "";
	var currentMatches = data.seckillInfo.flagMatches;
	var isDown = data.seckillInfo.down;
	
	var brandCount = 0;
	jQuery.each(data.seckillInfo.itemList, function(i, item){
		var itemIndex = (i + 1) + brandCount;
		var brand = data.seckillInfo.mBrandPos[itemIndex];
		if(brand != null && brand != undefined && brand.goodsList && brand.goodsList.length > 0) {
			content += '<li class="brand-li">';
			content += '<a href="/seckill/seckillBrandGoods?brandId=' + brand.id + '" onclick="return seckillMPingBrand(this.href)">';
			content += '<div class="brand-skill">';
			content += '<div class="brand-bgimg"><img src="/css/seckill/i/brand-bg.png"></div>';
			content += '<h3 class="brand-tit">';
			content += '<span class="brand-tit-lf"></span>';
			content += '<p class="brand-sm-tit"><strong>' + brand.titile + '</strong><span>' + brand.subTitle + '</span></p>';
			content += '<span class="brand-more">更多<i class="brand-icon"></i></span>';
			content += '</h3>';
			content += '<div class="brand-wrap">';
			content += '<div class="brand-box">';
			content += '<ul class="brand-wrap-list">';
			for(var brandIndex in brand.goodsList) {
				var brandItem = brand.goodsList[brandIndex]
				if(brandItem && brandItem.miaoSha) {
					content += '<li class="brand-iteam">';
				} else {
					content += '<li class="brand-iteam brand-no">';
				}
				content += '<span>';
				if(brandItem.imageurl) {
					if(brandItem.imageurl.indexOf("http://") != -1) {
						brandItem.imageurl = brandItem.imageurl.replace("http://", "//");
					} else if(brandItem.imageurl.indexOf("https://") != -1) {
						brandItem.imageurl = brandItem.imageurl.replace("https://", "//");
					}
				}
				content += '<div class="changeImg"><img src="/css/seckill/i/defaultGoodsImage.png" _src="' + brandItem.imageurl + '"></div>';
        		if(brandItem && brandItem.tagType && brandItem.tagText) {
        			var fontSize = "";
        			var itemTagName = "";
        			if(brandItem.tagText.length == 3) {
        				fontSize = "arrow-three";
        				itemTagName = brandItem.tagText;
        			} else if(brandItem.tagText.length == 4) {
        				fontSize = "";
        				itemTagName = brandItem.tagText.substr(0, 2) + "<br>" + brandItem.tagText.substr(2, brandItem.tagText.length);
        			} else {
        				fontSize = "";
        				itemTagName = brandItem.tagText;
        			}
        			
        			if(brandItem.tagType == 2) {
        				content += '<div class="skill-pic">';
        				content += '<em class="bskill-arrow arrow-yellow ' + fontSize + '">';
        				content += '<i>';
        				content += itemTagName;
        				content += '</i>';
        				content += '</em>';
        				content += '</div>';
        			} else if(brandItem.tagType == 4) {
        				content += '<div class="skill-pic">';
        				content += '<em class="bskill-arrow arrow-blue ' + fontSize + '">';
        				content += '<i>';
        				content += itemTagName;
        				content += '</i>';
        				content += '</em>';
        				content += '</div>';
        			} else if(brandItem.tagType == 3) {
        				content += '<div class="skill-pic">';
        				content += '<em class="skill-arrow ' + fontSize + '">';
        				content += '<i>';
        				content += itemTagName;
        				content += '</i>';
        				content += '</em>';
        				content += '</div>';
        			}
        		}
				if(brandItem.miaoSha) {
					content += '<div class="brand-bg"><p><span class="brand-price-now">￥' + brandItem.miaoShaPrice + '</span><del class="brand-price-old">￥' + brandItem.jdPrice + '</del></p></div>';
				} else {
					content += '<div class="brand-bg"><p>已抢完</p></div>';
				}
				content += '</span>';
				content += '</li>';
			}
			content += '</ul>';
			content += '</div>';
			content += '</div>';
			content += '</div>';
			content += '</a>';
			content += '</li>';
			brandCount++;
		}
		content += '<li class="bdr-bom">';
		content += '<a href="' + global_item_host + '/product/' + item.wareId + '.html" onClick="return seckillMPingCommoddity(this.href, \'' + item.wareId + '\')" >';
		
		content += '<div class="skill-pic">';
		if(item.imageurl) {
			if(item.imageurl.indexOf("http://") != -1) {
				item.imageurl = item.imageurl.replace("http://", "//");
			} else if(item.imageurl.indexOf("https://") != -1) {
				item.imageurl = item.imageurl.replace("https://", "//");
			}
		}
		content += '<div class="img"><img src="/css/seckill/i/defaultGoodsImage.png" _src="' + item.imageurl + '"></div>';
		if(item.tagType && item.tagText) {
			var fontSize = "";
			var itemTagName = "";
			if(item.tagText.length == 3) {
				fontSize = "arrow-three";
				itemTagName = item.tagText;
			} else if(item.tagText.length == 4) {
				fontSize = "";
				itemTagName = item.tagText.substr(0, 2) + "<br>" + item.tagText.substr(2, item.tagText.length);
			} else {
				fontSize = "";
				itemTagName = item.tagText;
			}
			
			if(item.tagType == 2) {
				content += '<em class="bskill-arrow arrow-yellow ' + fontSize + '">';
				content += '<i>';
				content += itemTagName;
				content += '</i>';
				content += '</em>';
			} else if(item.tagType == 4) {
				content += '<em class="bskill-arrow arrow-blue ' + fontSize + '">';
				content += '<i>';
				content += itemTagName;
				content += '</i>';
				content += '</em>';
			} else if(item.tagType == 3) {
				content += '<em class="skill-arrow ' + fontSize + '">';
				content += '<i>';
				content += itemTagName;
				content += '</i>';
				content += '</em>';
			}
		}
		
		content += '</div>';
		content += '<p class="g-title">' + (item.wname == null ? "" : item.wname) + '</p>';
		var first, last;
    	if(item.miaoShaPrice.indexOf(".") != -1) {
    		var full = item.miaoShaPrice.split(".");
    		first = full[0];
    		last = "." + full[1];
    	} else {
    		first = item.miaoShaPrice;
    		last = "";
    	}
		content += '<p class="g-price"><i class="doller">￥</i>' + first + '<span class="f-s-12">' + last + '</span></p>';
		content += '<div class="skill-price">';
		content += '<p class="g-price-odd">';
		content += '<del>￥' + (item.jdPrice == null ? "" : item.jdPrice) + '</del>';
		content += '</p>';
		if(currentMatches) {
			content += '<div class="skill-lod">';
			if(!isDown) {
				if(item.miaoSha) {
					if(item.soldRate) {
						content += '<span class="sale-count" id="sale-count-b">已秒<em>' + (item.soldRate == null ? "" : item.soldRate) + '%</em></span>';
						content += '<div id="progress-b" class="kill-progress">';
						content += '<div class="skill-pro-bg">';
						content += '<p class="skill-iteam-progress">';
						content += '<span class="skill-pro-insetbg">';
						content += '<span class="skill-iteam-pro"></span>';
						content += '</span>';
						content += '</p>';
						content += '</div>';
						content += '</div>';
					}
				} else {
					if(item.mTips) {
						content += '<span class="sale-count">' + (item.mTips == null ? "" : item.mTips) + '</span>';
					} else if(item.tips) {
						var tips = item.tips.substr(0, item.tips.length - 3) + "已秒杀完";
						content += '<span class="sale-count">' + (tips == null ? "" : tips) + '</span>';
					}
				}
			}
			content += '</div>';
		}
		content += '</div>';
		
		if(currentMatches) {
			if(item.miaoSha) {
				content += '<span class="skill-count">去秒杀</span>';
			} else {
				if(item.specialKill && item.specialKill == 1) {
					if(item.mpageAddress) {
						content += '<a class="kill-btn-side" href="' + item.mpageAddress + '.html" onClick="return seckillMPingMorespecialslae(this.href)">';
					} else if(item.shopId) {
						content += '<a class="kill-btn-side" href="' + global_ok_host.replace("%s", item.shopId) + '" onClick="return seckillMPingPreferential(this.href)">';
					} else {
						content += '<a class="kill-btn-side" href="' + global_item_host + '/product/' + item.wareId + '.html" onClick="return seckillMPingCommoddity(this.href, \'' + item.wareId + '\')">';
					}
					content += '<span class="skill-count kill-spestal">更多优惠</span>';
					content += '</a>';
					content += '<div class="mask">';
					content += '<p>秒杀完</p>';
					content += '</div>';
				} else {
					content += '<span class="skill-count skill-count-grey">秒杀完</span>';
				}
			}
		} else {
			content += '<span class="skill-count wait-kill">待秒杀</span>';
		}

		content += '</a>';
		if(item.specialKill && item.specialKill == 1 && item.miaoSha) {
			if(item.mpageAddress) {
				content += '<a class="skill-more-link" href="' + item.mpageAddress + '" onClick="return seckillMPingMorespecialslae(this.href)">更多特价 ></a>';
			} else if(item.shopId) {
				content += '<a class="skill-more-link" href="' + global_ok_host.replace("%s", item.shopId) + '" onClick="return seckillMPingPreferential(this.href)">全店优惠 > </a>';
			}
		}
		
		content += '</li>';
	});
	$('.seckilling').append(content);
};
SecKill.prototype = {
	initFn: function(){
		//$.loadImg.lazyLoad({rangeH:50,duration:200});
		// 初始化title内容
		//this.toSetInit();
		//倒计时部分
		this.countDown(global_time_remain,"#seckill_time");
		//首行时间置顶
		//this.fixPos();	
		//首行场次切换
		//this.switchTab();
		//返回顶部功能	
		//this.goBack2Top();
		//拉到最底部 进入下一场次
		//this.toLoadNextRound();
	},
	toSetInit: function(){
		// 根据title选中内容 改变Banner下方提示信息内容
		var curLi = this.oHeader.querySelector('.tap-list>li.cur');
		this.changeTitleInfo(curLi.children[0].innerHTML,curLi.children[1].innerHTML);
		var curName = curLi.children[3].innerHTML;
		var gid = $(curLi).attr("id");
	},
	fixPos:function(){//场次置顶
		var _this = this;
		window.addEventListener('scroll',function(e){
			var m_common_header_height = _this.m_common_header.offsetHeight;
			var scrollTop = document.documentElement.scrollTop || document.body.scrollTop;
			
			if(scrollTop>=m_common_header_height){
				_this.oHeader.style.position = 'fixed';
			}else{
				_this.oHeader.style.position = 'static';
			}
				
			return false;
		},false);		
	},
	toLoadNextRound: function(){
		var loading=document.querySelector('#layout_toClient');
		loading.addEventListener('click',function(){
			$('#topfixed .tap-list  li.cur').next().click();
		},false);
	},
	switchTab: function(){//场次切换
		var aLi = this.oHeader.children[0].children;
		
		var _this = this;
		var progressTimout;
		for(var i=0;i<aLi.length;i++){
			(function(idx){
				aLi[idx].addEventListener('click',function(){
					var curTime,curStatus,curName,gid,loading,liLength;
					for(var i=0;i<aLi.length;i++){
						aLi[i].classList.remove('cur');
					}
					this.classList.add('cur');
					// 根据title选中内容 改变Banner下方提示信息内容
					curTime = this.children[2].innerHTML;
					curStatus = this.children[1].innerHTML;
					curName = this.children[3].innerHTML;
					gid = $(this).attr("id");
					
					seckillMPingTag("http://coupon.m.jd.com/seckill/seckillList.json", curName, gid);
					_this.changeTitleInfo(curTime,curStatus);
					// 如果是最后一个 隐藏 “点击进入下一场次”
					loading=document.querySelector('#layout_toClient');
					liLength = $('#topfixed .tap-list  li').length;
					if($('#topfixed .tap-list  li.cur').index() == (liLength-1)){
						loading.style.display = 'none' ;
					}else{
						loading.style.display = 'block' ;
					}
					//不同场次倒计时切换
					clearInterval(_this.timer);
					window.scroll(0,0);
					document.querySelector('#indexToTop').style.display = 'none' ;
					var param = "gid=" + gid;
					if(global_wareId) {
						param += "&wareId=" + global_wareId;
					}
					jQuery.ajax({
						type: "get",
						data : param,
						dataType: "json",
						url: '/seckill/seckillList.json',
						beforeSend: function(XMLHttpRequest) {
							$('#seckill-body').hide();
							$('.errPic').show();
							$('.seckilling').empty();
						},
						success: function(data, textStatus){
							if(textStatus=='success'){
								if(data != null && data.seckillInfo != null){
									parseSeckillList(data);
									jQuery.each(data.seckillInfo.matchesList, function(i, matches) {
										if(matches.gid == gid) {
											if(matches.operateImg) {
												if(matches.operateImg.indexOf("http://") != -1) {
													matches.operateImg = matches.operateImg.replace("http://", "//");
												} else if(matches.operateImg.indexOf("https://") != -1) {
													matches.operateImg = matches.operateImg.replace("https://", "//");
												}
												$('.big-ad-column').show();
												$('.big-ad-column>a').attr("href", matches.jumpUrl);
												$('.big-ad-column>a>img').attr("src", matches.operateImg);
											} else {
												$('.big-ad-column').hide();
											}
											return false;
										}
									});
									var seckillCommon = new SeckillCommon();
									seckillCommon.initByType();
									$('.errPic').hide();
									$('#seckill-body').show();
									//_this.seckillSecond = data.seckillInfo.timeRemain;
									_this.countDown(data.seckillInfo.timeRemain,"#seckill_time");
									clearTimeout(progressTimout);
									progressTimout = setTimeout(function() {
										_this.progress();
									}, 30);
								}
							}
						},
						complete: function(XMLHttpRequest, textStatus){	
							$(".brand-iteam").jdImg();
							$.loadImg.lazyLoad({rangeH:50,duration:200});
							isLoadingMore = false;
						},
						error: function(){					
						}
					});
				},false);

			})(i);
		}
	},
	changeTitleInfo: function(curTime,curStatus){
		if(curStatus==this.constantData.titleStatusWill){//'即将开场'
			this.seckillBuyText.innerHTML = this.constantData.willBuyText;
			this.seckillBuyText.classList.add("buy-txt-black");
			this.seckillTitle.innerHTML= this.constantData.infoMsgWillSeckill;//'秒杀预告'
			this.staticTxtEnd.innerHTML= this.constantData.infoMsgBegin;//'距开始'
//			$('.seckilling').hide();
//			$('.will-seckill').show();
		}else if(curStatus==this.constantData.titleStatusBeing){//'秒杀中'
			this.seckillBuyText.innerHTML = this.constantData.buyText;
			this.seckillBuyText.classList.remove("buy-txt-black");
			this.seckillTitle.innerHTML= this.constantData.infoMsgSeckilling;//'秒杀中'
			this.staticTxtEnd.innerHTML= this.constantData.infoMsgEnd;//'距结束'
//			$('.seckilling').show();
//			$('.will-seckill').hide();
		}
		this.seckillRoundNo.innerHTML= curTime;
	},
	countDown: function(sys_second,id){//计时器 倒计时
		var _this = this ;
		clearInterval(_this.timer);
		secFn();
		_this.timer = setInterval(secFn, 1000);
        
		function secFn(){
		    if (sys_second > 0) { //10675_1448283861121
		       
			    var seckillListTime = window.localStorage.getItem('seckill_list');
				if(seckillListTime!=undefined && seckillListTime!=null && seckillListTime!=""){
				    var nowTime=new Date().getTime();
				    var backTime=seckillListTime.split("_")[1];
					var goTime=seckillListTime.split("_")[0];
					if(_this.seckillSecond==goTime){
						var sTime=parseInt((nowTime-backTime)/1000);
						if(sTime>1){
						    sys_second=sys_second-sTime+1;
						}
					}
				}
				sys_second -= 1; 
				var hour = Math.floor((sys_second / 3600) % 24); 
				var minute = Math.floor((sys_second / 60) % 60); 
				var second = Math.floor(sys_second % 60); 
				hour=hour<10?"0"+hour:hour;//计算小时 
				minute=minute<10?"0"+minute:minute;//计算分钟 
				second=second<10?"0"+second:second;//计算秒杀 

				var arrTime = $(id).find('span.seckill-time'); 
				$(arrTime[0]).html(hour);
				$(arrTime[1]).html(minute);
				$(arrTime[2]).html(second);
				try{
					window.localStorage.setItem('seckill_list',_this.seckillSecond+'_'+new Date().getTime());
				}catch(e){
//					console.log('用户是safri无痕浏览模式');
				}
			} else { 
				clearInterval(_this.timer);
				var t=Math.floor(Math.random()*3)
				var ts=setInterval(function(){
		    		if(t>0){
		    		   t-=1;
		    		}else{
					   clearInterval(ts);
					   //$("#gid").val("");
		               //$("#seckillForm").submit();
					}
		    	},1000);
			} 
		}
	},
	goBack2Top:function () {//回到顶部功能
	    var oUP = document.querySelector('#indexToTop');
	    var CLICK = 'click';
	    var evts = {
	        init: function () {
	            evts.scrollEvt();
	            evts.toTopEvt();
	        },
	        toTopEvt: function () {
	            oUP.addEventListener(CLICK, function () {
	                scroll(0, 0);
	                oUP.style.display = 'none';
	            }, false);
	        },
	        scrollEvt: function () {
	            window.addEventListener('scroll', function () {
	                var clientHeight = document.documentElement.clientHeight || document.body.clientHeight;
	                var scrollTop = document.documentElement.scrollTop || document.body.scrollTop;
	                if (scrollTop > clientHeight) {
	                    oUP.style.display = 'block';
	                } else {
	                    oUP.style.display = 'none';
	                }
	            }, false);
	        }
	    }
	    return evts.init();
	},
	progress:function() {
		$('.sale-count>em').each(function(index) {
			$($('.skill-iteam-pro')[index]).width($(this).html());
		});
	}
};
