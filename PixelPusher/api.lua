function cls(color)
	color = color or 0
	display:ClearBuffer(color)
end

function plot(x, y, color)
	color = color or 5
	display:SetPixel(x, y, color)
end

function rect(x, y, xx, yy, color)
	color = color or 5
	display:DrawRect(x, y, xx, yy, color)
end

function circ(x, y, r, color)
	color = color or 5
	display:DrawCircle(x, y, r, color)
end

function line(x, y, xx, yy, color)
	color = color or 5
	display:DrawLine(x, y, xx, yy, color)
end