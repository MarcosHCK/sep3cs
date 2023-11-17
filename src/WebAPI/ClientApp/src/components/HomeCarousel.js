/* Copyright (c) 2023-2025
 * This file is part of sep3cs.
 *
 * sep3cs is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * sep3cs is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with sep3cs. If not, see <http://www.gnu.org/licenses/>.
 */
import './HomeCarousel.css'
import { Carousel, CarouselCaption, CarouselControl, CarouselIndicators, CarouselItem } from 'reactstrap'
import React, { useState } from 'react'

export function HomeCarousel (props)
{
  const [activeIndex, setActiveIndex] = useState (0)
  const [animating, setAnimating] = useState (false)
  const { items, ...rest } = props

  const itemsKeyed = items.map ((item) => ({ caption : item.caption, key : item.src, src : item.src, }))
  const itemsCount = items.length

  const next = () => animating ? 0 : setActiveIndex (activeIndex === itemsCount - 1 ? 0 : activeIndex + 1)
  const previous = () => animating ? 0 : setActiveIndex (activeIndex === 0 ? itemsCount - 1 : activeIndex - 1)
  const goToIndex = (index) => animating ? 0 : setActiveIndex (index)

  return (
    <Carousel activeIndex={activeIndex} next={next} previous={previous} className={'home-carousel'} {...rest}>
      <CarouselIndicators activeIndex={activeIndex} items={itemsKeyed} onClickHandler={goToIndex} />

        {
          itemsKeyed.map ((item, index) => (
            <CarouselItem key={item.src} onExited={() => setAnimating (false)} onExiting={() => setAnimating (true)}>
              <img src={item.src} alt={`slide-${index}`} />
              <CarouselCaption captionHeader={item.caption} captionText={item.caption} />
            </CarouselItem>))
        }

      <CarouselControl direction='prev' directionText={'Previous'} onClickHandler={previous} />
      <CarouselControl direction='next' directionText={'Next'} onClickHandler={next} />
    </Carousel>) 
}
