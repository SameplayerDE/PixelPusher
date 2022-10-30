local x = 64
local y = 64

function _draw()
cls()
rect(x - 4, y - 4, x + 4, y + 4, 13)
circ(x, y, 10, 3)
line(0, 0, 128, 128, 7)
end

function _update()
	if btn(1) then
		x = x + 1
	end
end